using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using LCAVPCore.Processors;
using LCAVPCore.Registration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LCAVPCore
{
	public class LCAVPSubmissionProcessor : ILCAVPSubmissionProcessor
	{
		private const string CAVS_VERSION = "21.4";

		private readonly ISubmissionLogger _submissionLogger;
		private readonly INewSubmissionProcessor _newSubmissionProcessor;
		private readonly IChangeSubmissionProcessor _changeSubmissionProcessor;
		private readonly IUpdateSubmissionProcessor _updateSubmissionProcessor;
		private readonly IModuleProcessor _moduleProcessor;
		private readonly IOEProcessor _oeProcessor;
		private readonly IOrganizationProcessor _organizationProcessor;
		private readonly IPersonProcessor _personProcessor;
		private readonly IValidationProcessor _validationProcessor;
		private readonly IDataProvider _dataProvider;
		private readonly string _extractedFilesRoot;
		private readonly string _processedFilesRoot;

		public LCAVPSubmissionProcessor(ISubmissionLogger submissionLogger, INewSubmissionProcessor newSubmissionProcessor, IChangeSubmissionProcessor changeSubmissionProcessor, IUpdateSubmissionProcessor updateSubmissionProcessor, IModuleProcessor moduleProcessor, IOEProcessor oeProcessor, IOrganizationProcessor organizationProcessor, IPersonProcessor personProcessor, IValidationProcessor validationProcessor, IDataProvider dataProvider, IConfiguration configuration)
		{
			_submissionLogger = submissionLogger;
			_newSubmissionProcessor = newSubmissionProcessor;
			_changeSubmissionProcessor = changeSubmissionProcessor;
			_updateSubmissionProcessor = updateSubmissionProcessor;
			_moduleProcessor = moduleProcessor;
			_oeProcessor = oeProcessor;
			_organizationProcessor = organizationProcessor;
			_personProcessor = personProcessor;
			_validationProcessor = validationProcessor;
			_dataProvider = dataProvider;
			_extractedFilesRoot = configuration.GetValue<string>("LCAVP:ExtractedFilesRoot");
			_processedFilesRoot = configuration.GetValue<string>("LCAVP:ProcessedFilesRoot");
		}

		public SubmissionProcessingResult Process(string filePath)
		{
			string processedFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_{Path.GetFileName(filePath)}";

			FileExtractionResult extractionResult = null;
			SubmissionProcessingResult submissionProcessingResult = new SubmissionProcessingResult();

			//Validate that file name conforms to the rules
			//Is it a zip file - at least in name
			if (!filePath.EndsWith(".zip"))
			{
				submissionProcessingResult.Errors.Add("Not a zip file");
			}

			//Get some metadata out of the inf file
			(bool Found, string CAVSVersion, string LabName, string LabPOC, string LabPOCEmail, string NVLAPCode, string SubmissionCode, string UniqueNumber) = ExtractINFMetadata(filePath);

			//Determine if this is a New, Update, or Change submission
			SubmissionType submissionType = DetermineSubmissionType(filePath, SubmissionCode);

			if (submissionType == SubmissionType.Unknown)
			{
				submissionProcessingResult.Errors.Add("Unable to determine submission type");
			}

			//Now can check the CAVS version - if a Change that didn't include an inf, or anything 21.X, let it slide
			//First do a bit of manipulation on it in case they did "CAVS21.4" or "I used CAVS 21.4"
			bool versionIs21X = false;
			if (CAVSVersion != null)    //Because regex will blow if run on a null...
			{
				Regex cavsVersionPattern = new Regex(@"21\.\d");
				Match match = cavsVersionPattern.Match(CAVSVersion);
				if (match.Success)
				{
					CAVSVersion = match.Groups[0].Value;
					versionIs21X = true;
				}
			}

			//Now actually validate it
			if (submissionProcessingResult.Success)
			{
				if ((submissionType == SubmissionType.New || submissionType == SubmissionType.Update) && CAVSVersion != CAVS_VERSION)   //New or Update must be proper version
				{
					submissionProcessingResult.Errors.Add($"Does not use version {CAVS_VERSION} of CAVS");
				}
				else if (submissionType == SubmissionType.Change && CAVSVersion != null && !versionIs21X)   //Change can be null or anything 21.X
				{
					submissionProcessingResult.Errors.Add($"Does not use version 21.X of CAVS");
				}
			}


			//File appears valid, now can attempt to extract it
			if (submissionProcessingResult.Success)
			{
				//Extract the zip file
				FileHandler fileHandler = new FileHandler(submissionType, _extractedFilesRoot);
				extractionResult = fileHandler.ExtractZip(filePath);

				if (!extractionResult.Success)
				{
					submissionProcessingResult.Errors = extractionResult.Errors;
				}
			}

			//Attempt to process the extracted files
			if (submissionProcessingResult.Success)
			{
				//Process (or not)
				switch (submissionType)
				{
					case SubmissionType.New:
						submissionProcessingResult.ProcessingResults.AddRange(_newSubmissionProcessor.Process(extractionResult.ExtractionPath));
						break;

					case SubmissionType.Update:
						//Need to do both an update and a change
						submissionProcessingResult.ProcessingResults.AddRange(_changeSubmissionProcessor.Process(extractionResult.ExtractionPath));
						submissionProcessingResult.ProcessingResults.AddRange(_updateSubmissionProcessor.Process(extractionResult.ExtractionPath));
						break;

					case SubmissionType.Change:
						submissionProcessingResult.ProcessingResults.AddRange(_changeSubmissionProcessor.Process(extractionResult.ExtractionPath));
						break;

					default:
						submissionProcessingResult.Errors.Add("Unable to determine submission type");
						break;
				}
			}

			int submissionLogID;
			string submissionID = $"{NVLAPCode}-{SubmissionCode}-{UniqueNumber}";

			//Deal with the results
			if (submissionProcessingResult.Success)     //Everything about the file was valid, but there may be issues from processing the contents
			{
				//Add any individual errors to the overall collection
				submissionProcessingResult.Errors.AddRange(submissionProcessingResult.ProcessingResults.SelectMany(r => r.Errors));

				//Log the submission
				submissionLogID = LogSubmission(submissionID, LabName, LabPOC, LabPOCEmail, submissionType, submissionProcessingResult.Success, processedFileName, extractionResult.ExtractionPath, submissionProcessingResult.Errors);

				//Assuming submission logged successfully, log the results of each individual inf file that was processed, each producing its own "registration"
				if (submissionLogID != -1)
				{
					bool haveError = false;

					foreach (ProcessingResult processingResult in submissionProcessingResult.ProcessingResults)
					{
						if (processingResult.Errors.Count > 0) haveError = true;
						LogSubmissionRegistration(submissionLogID, processingResult);
					}

					//Process everything if there were no errors
					if (!haveError)
					{
						foreach (ProcessingResult processingResult in submissionProcessingResult.ProcessingResults)
						{
							switch ((processingResult.Type, processingResult.WorkflowType))
							{
								case (ProcessingType.Change, WorkflowType.Module):
								case (ProcessingType.Update, WorkflowType.Module):
									_moduleProcessor.Update((Module)processingResult.TheThingy);
									break;

								case (ProcessingType.Change, WorkflowType.OE):
								case (ProcessingType.Update, WorkflowType.OE):
									_oeProcessor.Update((OperationalEnvironment)processingResult.TheThingy);
									break;

								case (ProcessingType.Change, WorkflowType.Organization):
								case (ProcessingType.Update, WorkflowType.Organization):
									_organizationProcessor.Update((Vendor)processingResult.TheThingy);
									break;

								case (ProcessingType.Change, WorkflowType.Person):
								case (ProcessingType.Update, WorkflowType.Person):
									_personProcessor.Update((Contact)processingResult.TheThingy);
									break;

								case (ProcessingType.New, WorkflowType.Validation):
									var result = _validationProcessor.Create((NewRegistrationContainer)processingResult.TheThingy);

									//Update the submission log with the validation ID that was created - this may get used by future submissions in prereqs
									_submissionLogger.UpdateValidationIDForSubmissionLogID(submissionLogID, result.ID);

									//Populate the validation number on the result
									submissionProcessingResult.ValidationNumber = _dataProvider.GetValidationNumberForValidationID(result.ID);

									break;

								case (ProcessingType.Update, WorkflowType.Validation):
									_validationProcessor.Update((UpdateRegistrationContainer)processingResult.TheThingy);
									break;
							}
						}
					}
				}
			}
			else
			{
				//Log the submission
				submissionLogID = LogSubmission(submissionID, LabName, LabPOC, LabPOCEmail, submissionType, submissionProcessingResult.Success, processedFileName, extractionResult?.ExtractionPath, submissionProcessingResult.Errors);
			}

			//Move the file to the processed location
			string destinationPath = Path.Combine(_processedFilesRoot, processedFileName);

			File.Move(filePath, destinationPath);

			//Populate some of the fields on the result
			submissionProcessingResult.SubmissionType = submissionType;
			submissionProcessingResult.LabName = LabName;
			submissionProcessingResult.LabPOC = LabPOC;
			submissionProcessingResult.LabPOCEmail = LabPOCEmail;
			submissionProcessingResult.SubmissionID = submissionLogID;

			return submissionProcessingResult;
		}

		private SubmissionType DetermineSubmissionType(string filePath, string submissionCode)
		{
			Regex submissionTypePattern = new Regex(@".+-(?<type>[NUC])-.+$");          //(@" ^\d{6}-\d-(?<type>[NUC])-.+$");
			Match match = submissionTypePattern.Match(Path.GetFileNameWithoutExtension(filePath));

			if (match.Success)
			{
				return SubmissionTypeFromString(match.Groups["type"].Value);
			}
			else
			{
				//Go with what was in the inffile instead of in the file name
				return SubmissionTypeFromString(submissionCode);
			}
		}

		private (bool Found, string CAVSVersion, string LabName, string LabPOC, string LabPOCEmail, string NVLAPCode, string SubmissionCode, string UniqueNumber) ExtractINFMetadata(string filePath)
		{
			//Starting with this path, try to extract the needed metadata from inf file
			using ZipArchive archive = new ZipArchive(File.OpenRead(filePath), ZipArchiveMode.Read);
			return RecursiveINFMetadataSearch(archive);
		}

		private (bool Found, string CAVSVersion, string LabName, string LabPOC, string LabPOCEmail, string NVLAPCode, string SubmissionCode, string UniqueNumber) RecursiveINFMetadataSearch(ZipArchive archive)
		{
			(bool Found, string CAVSVersion, string LabName, string LabPOC, string LabPOCEmail, string NVLAPCode, string SubmissionCode, string UniqueNumber) infData = (false, null, null, null, null, null, null, null);

			//Look for an inf file in this zip file
			ZipArchiveEntry infFile = archive.Entries.FirstOrDefault(e => e.Name.EndsWith(".inf") && !e.Name.StartsWith("._"));     //._ thing filters out garbage from zips from Macs

			if (infFile != null)
			{
				//Found an inf file, so now extract the data we care about
				infData.Found = true;

				using StreamReader reader = new StreamReader(infFile.Open());
				string line;
				int linesFound = 0;     //Hacky way to keep track of whether or not all lines we care about here have been found, without having to write 5 booleans

				//Read a line at a time from the stream until finding the line we care about
				while ((line = reader.ReadLine()) != null && linesFound != 7)
				{
					if (line.StartsWith("CAVS_Tool_Version"))
					{
						infData.CAVSVersion = line.Substring(18).Trim();
						linesFound++;
					}

					if (line.StartsWith("Lab_Name="))
					{
						infData.LabName = line.Substring(9).Trim();
						linesFound++;
					}

					if (line.StartsWith("Lab_POC="))
					{
						infData.LabPOC = line.Substring(8).Trim();
						linesFound++;
					}

					if (line.StartsWith("Lab_POC_Email="))
					{
						infData.LabPOCEmail = line.Substring(14).Trim();
						linesFound++;
					}

					if (line.StartsWith("ImplCodePart1_NVLABCode="))    //Yes, spelled wrong
					{
						infData.NVLAPCode = line.Substring(24).Trim();
						linesFound++;
					}

					if (line.StartsWith("ImplCodePart2_SubmissionCode="))
					{
						infData.SubmissionCode = line.Substring(29).Trim();
						linesFound++;
					}

					if (line.StartsWith("ImplCodePart3_UniqueNumber="))
					{
						infData.UniqueNumber = line.Substring(27).Trim();
						linesFound++;
					}
				}
			}
			else
			{
				//No inf file in this zip, so search any nested zip files. Hopefully there aren't any.
				IEnumerable<ZipArchiveEntry> nestedZipFiles = archive.Entries.Where(e => e.Name.EndsWith(".zip") && !e.Name.StartsWith("._"));      //._ thing filters out garbage from zips from Macs

				//Iterate through the child zip files until an inf file is found
				foreach (ZipArchiveEntry zipFile in nestedZipFiles)
				{
					//Open the nested zip file as a stream
					using ZipArchive childArchive = new ZipArchive(zipFile.Open(), ZipArchiveMode.Read);
					infData = RecursiveINFMetadataSearch(childArchive);
					if (infData.Found)
					{
						break;
					}
				}
			}

			return infData;
		}

		private string SubmissionTypeString(SubmissionType type) => type switch
		{
			SubmissionType.New => "N",
			SubmissionType.Update => "U",
			SubmissionType.Change => "C",
			_ => null,
		};

		private SubmissionType SubmissionTypeFromString(string value) => value switch
		{
			"N" => SubmissionType.New,
			"U" => SubmissionType.Update,
			"C" => SubmissionType.Change,
			_ => SubmissionType.Unknown,
		};

		private int LogSubmission(string submissionID, string labName, string labPOC, string labPOCEmail, SubmissionType submissionType, bool success, string zipFileName, string extractedFileLocation, List<string> errors)
		{
			string errorJson = errors.Count == 0 ? null : JsonConvert.SerializeObject(errors);      //If no errors, an empty list will be in the serialized output, this prevents anything from being returned in that case. Kludge.

			return _submissionLogger.LogSubmission(submissionID, labName, labPOC, labPOCEmail, SubmissionTypeString(submissionType), DateTime.Now.ToUniversalTime(), success ? 1 : 0, zipFileName, extractedFileLocation, errorJson);
		}

		private void LogSubmissionRegistration(int submissionID, ProcessingResult processingResult)
		{
			string errorJson = processingResult.Errors.Count == 0 ? null : JsonConvert.SerializeObject(processingResult.Errors);      //If no errors, an empty list will be in the serialized output, this prevents anything from being returned in that case. Kludge.

			_submissionLogger.LogSubmissionRegistration(submissionID, processingResult.Success ? 1 : 0, processingResult.WorkflowType, processingResult.RegistrationJson, errorJson);
		}
	}
}
