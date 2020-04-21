using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.KAS;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KAS
{
	public class KAS_ECC_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "800-56a_ECC_SummaryFile.txt";
		private string _summaryFilePath;

		public KAS_ECC_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public KAS_ECC_Results Extract()
		{
			KAS_ECC_Results results = new KAS_ECC_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			List<PassFailResult> sectionResults;

			int i = 0;
			while (i < lines.Length - 1)    //Since we're going to look for the header and the line after, and the last line of the file being a header would be useless, just stop one before the length (which means it is always safe to look at the next line without overflow)
			{
				//Look for a line starting with a * (a header line) followed by a line starting with [ECC (an ECC test result) - if the next line starts with something other than [ECC, it is the ECC CDH component result, which should be ignored here
				if (lines[i].StartsWith("*") && lines[i + 1].StartsWith("[ECC"))
				{
					//Get the results under this header
					sectionResults = GetTestResults(lines, i);

					//Now need to figure out what those result lines represent. Depends on the header and the name of the first test. If neither functionality or validity header, we don't care about it.
					if (sectionResults.Count > 0)
					{
						if (lines[i].Contains("Functionality"))
						{
							//Test names are all [ECC_PartWeCareAbout_OtherStuff], so split on the _ and take the 2nd member of that array to identify what scheme we have data for
							switch (sectionResults[0].TestName.Split("_".ToCharArray())[1])
							{
								case "EPHEMUNIFIED":
									results.EphemeralUnified_Functionality = sectionResults;
									break;
								case "FULLMQV":
									results.FullMQV_Functionality = sectionResults;
									break;
								case "FULLUNIFIED":
									results.FullUnified_Functionality = sectionResults;
									break;
								case "ONEPASSDH":
									results.OnePassDH_Functionality = sectionResults;
									break;
								case "ONEPASSMQV":
									results.OnePassMQV_Functionality = sectionResults;
									break;
								case "ONEPASSUNIFIED":
									results.OnePassUnified_Functionality = sectionResults;
									break;
								case "STATICUNIFIED":
									results.StaticUnified_Functionality = sectionResults;
									break;
								default:
									break;	//Should never happen
							}
						}
						else if (lines[i].Contains("Validity"))
						{
							switch (sectionResults[0].TestName.Split("_".ToCharArray())[1])
							{
								case "EPHEMUNIFIED":
									results.EphemeralUnified_Validity = sectionResults;
									break;
								case "FULLMQV":
									results.FullMQV_Validity = sectionResults;
									break;
								case "FULLUNIFIED":
									results.FullUnified_Validity = sectionResults;
									break;
								case "ONEPASSDH":
									results.OnePassDH_Validity = sectionResults;
									break;
								case "ONEPASSMQV":
									results.OnePassMQV_Validity = sectionResults;
									break;
								case "ONEPASSUNIFIED":
									results.OnePassUnified_Validity = sectionResults;
									break;
								case "STATICUNIFIED":
									results.StaticUnified_Validity = sectionResults;
									break;
								default:
									break;	//Should never happen
							}
						}
					}

					//Increment the loop by the number of results lines parsed. Since i was the index of the header line, this will leave i as the index of the last result line before the next header, meaning the incrementer outside this if is correct regardless of the number of results found
					i += sectionResults.Count;
				}

				i++;	//Move to the next line
			}

			return results;
		}

		private List<PassFailResult> GetTestResults(string[] lines, int headerIndex)
		{
			List<PassFailResult> results = new List<PassFailResult>();

			string[] parts;
			int i = headerIndex + 1;    //Start at the line after the *** header line

			while (i < lines.Length && lines[i].StartsWith("["))    //Stop the loop when the line doesn't start with [, which means we've reached the next section
			{
				//Split based on the tabs, should give us the test name and the result value
				parts = lines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				//Create a new result object based on the test name and whether or not it passed
				results.Add(new PassFailResult(parts[0], parts[1].Trim() == "PASSED"));

				i++;
			}

			return results;
		}
	}
}