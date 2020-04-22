using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public class NewSubmissionProcessor : INewSubmissionProcessor
	{
		private readonly IAlgorithmFactory _algorithmFactory;
		private readonly IAlgorithmEvaluatorFactory _algorithmEvaluatorFactory;
		private readonly IInfFileParser _infFileParser;
		private readonly IDataProvider _dataProvider;

		public NewSubmissionProcessor(IAlgorithmFactory algorithmFactory, IAlgorithmEvaluatorFactory algorithmEvaluatorFactory, IInfFileParser infFileParser, IDataProvider dataProvider)
		{
			_algorithmFactory = algorithmFactory;
			_algorithmEvaluatorFactory = algorithmEvaluatorFactory;
			_infFileParser = infFileParser;
			_dataProvider = dataProvider;
		}


		public List<ProcessingResult> Process(string submissionRoot)
		{
			//Get the Module. This will be the same across everything within this submission, so just take 1 inf file. Hope it is valid...
			Module module = GetModule(Directory.GetFiles(submissionRoot, "*.inf", SearchOption.AllDirectories).First());

			//Process/Validate each of the inf/summary folders in this submission. This will build a collection of results.
			List<ProcessingResult> results = new List<ProcessingResult>();

			//With the change to 1 registration per submission (for new stuff), there's just 1 processing result object
			ProcessingResult processingResult = new ProcessingResult { Type = ProcessingType.New, WorkflowType = WorkflowType.Validation };

			//Create the object to serialize into the registration
			NewRegistrationContainer registration = new NewRegistrationContainer
			{
				Module = module
			};

			List<Scenario> PotentialScenarios = new List<Scenario>();

			foreach (string folder in Directory.EnumerateDirectories(submissionRoot))
			{
				//Evaluate the inf file in the folder - assuming it is there
				InfEvaluationResult infEvaluationResult = EvaluateInfSummaryFolder(folder);

				//If no inf file in the folder the result will be null, so ignore it
				if (infEvaluationResult != null)
				{
					//Add any inf evaluation errors to the error collection
					if (!infEvaluationResult.IsValidFile)
					{
						if (infEvaluationResult.InfFile == null)
						{
							processingResult.Errors.AddRange(infEvaluationResult.Errors);
						}
						else
						{
							processingResult.Errors.AddRange(infEvaluationResult.Errors.Select(x => $"{infEvaluationResult.InfFile.FolderNumber} - {x}"));
						}
					}
					else
					{
						//if it was at least a valid inf file, then want to build the registration, whether or not it passed

						//Add any special instructions
						if (!string.IsNullOrEmpty(infEvaluationResult.SpecialInstructions))
						{
							processingResult.Errors.Add(infEvaluationResult.SpecialInstructions);
						}

						//Add any validation errors
						if (!infEvaluationResult.Success)
						{
							processingResult.Errors.AddRange(infEvaluationResult.AlgorithmResults.SelectMany(x => x.FailureMessages).Select(x => $"{infEvaluationResult.InfFile.FolderNumber} - {x}"));
						}

						//Parse the inf file into the different algorithms
						List<IAlgorithm> algorithms = BuildAlgorithms(infEvaluationResult.InfFile);

						PotentialScenarios.Add(new Scenario
						{
							OEs = new List<OperationalEnvironment> { GetOE(infEvaluationResult.InfFile) },
							Algorithms = algorithms.Select(a => new AlgorithmCapabilities(a)).ToList()
						});
					}
				}
			}

			//Add any errors found when building the algorithm objects
			processingResult.Errors.AddRange(registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Algorithm.Errors));

			//Do the lookups on any prereqs that reference submission IDs - and mark as unresolved if dependent submission has not been approved
			bool havePendingPrereq = false;
			foreach (var scenario in registration.Scenarios)
			{
				foreach (var algorithm in scenario.Algorithms)
				{
					foreach (var prereq in algorithm.Prerequisites.Where(x => !string.IsNullOrEmpty(x.SubmissionID)))
					{
						//Look it up
						long validationID = _dataProvider.GetValidationIDForSubmissionID(prereq.SubmissionID);

						//Update the prereq, or flag an error
						if (validationID == -1)
						{
							prereq.IsUnprocessedSubmission = true;
							havePendingPrereq = true;
						}
						else
						{
							prereq.ValidationRecordID = validationID;
						}
					}
				}
			}

			//If there are any prereqs that reference submission IDs that have not been approved, throw an error
			if (havePendingPrereq)
			{
				string submissions = string.Join(", ", registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites ?? new List<Prerequisite>()).Where(x => x.IsUnprocessedSubmission).Select(x => x.SubmissionID));
				processingResult.Errors.Add($"Cannot be processed because it depends on other submissions that have not been approved. Retry after you have approved submission(s) {submissions}");
			}


			//Do the combining of like scenarios. Will use the resulting list on the registration
			List<(OperationalEnvironment oe, List<AlgorithmCapabilities> algorithms, string algorithmJson)> foo = PotentialScenarios.Select(x => (x.OEs[0], x.Algorithms, JsonConvert.SerializeObject(x.Algorithms))).ToList();

			//Get the distinct algorithms JSON from that collection
			List<string> distinctRegistrations = foo.Select(x => x.algorithmJson).Distinct().ToList();

			//Want to group the OEs by the registrations, make a new scenario for each diferent registration - probably not often that there's > 1
			foreach (string registrationJson in distinctRegistrations)
			{
				//Get one of the algorithm collections to use for this scenario
				List<AlgorithmCapabilities> algorithmCapabilities = foo.First(x => x.algorithmJson == registrationJson).algorithms;

				//Get all the OEs that have this same registration
				List<OperationalEnvironment> oes = foo.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

				//Add the scenario
				registration.Scenarios.Add(new Scenario
				{
					Algorithms = algorithmCapabilities,
					OEs = oes
				});
			}

			//Put this object on the processing result
			processingResult.TheThingy = registration;

			//Serialize the registration to JSON to include in the processing result
			processingResult.RegistrationJson = JsonConvert.SerializeObject(registration);

			//Add the result to the collection (which will now only have a single member, but still...)
			results.Add(processingResult);

			return results;
		}


		private InfEvaluationResult EvaluateInfSummaryFolder(string folder)
		{
			InfEvaluationResult result = new InfEvaluationResult();

			//Find the name of inf file in the folder. Use a try/catch to handle the various reasons why it couldn't be found (directory not there, no inf file, multiple inf files...)
			string infFileName = null;

			try
			{
				infFileName = Directory.EnumerateFiles(folder).Single(f => f.EndsWith(".inf"));
			}
			catch
			{
				//Can fail for 0 or > 1 inf file - the former is no problem, the latter is a problem
				if (Directory.EnumerateFiles(folder).Any(f => f.EndsWith(".inf")))
				{
					result.Errors.Add($"Multiple inf files detected within {folder} - there can be only one");
					return result;
				}
				else
				{
					return null;        //Just means there was an extranneous folder
				}
			}

			//Get and parse the inf file
			InfFile infFile = _infFileParser.Parse(Path.Combine(folder, infFileName));

			if (!infFile.Valid)
			{
				result.Errors.Add($"Parse failed of {folder}/{infFileName}");
				return result;
			}

			//Populate the folder property of the object, in case we need to know which inf file this error came from (like in error reporting)
			infFile.Folder = folder;

			//Add the valid inf file to the result - we'll need this for building the registration later
			result.InfFile = infFile;

			//Check for special instructions in the inf file
			string specialInstructions = ParseSpecialInstructions(infFile);
			if (specialInstructions != null)
			{
				result.SpecialInstructions = $"Special Instructions: {specialInstructions}";
			}

			//Check to make sure they selected something...
			if (infFile.Algorithms.Count == 0)
			{
				result.Errors.Add("No algorithms were selected/tested");
				return result;
			}

			//Evaluate the inf file, comparing the configuration to what the summary files say was tested

			//Evaluate each supported algorithm parsed out of the inf file
			foreach (InfAlgorithm algo in infFile.Algorithms)
			{
				//Get the appropriate algorithm evaluator
				IAlgorithmEvaluator evaluator = _algorithmEvaluatorFactory.GetEvaluator(algo, folder);

				if (evaluator == null)
				{
					result.Errors.Add($"Unable to load evaluator for algorithm {algo.AlgorithmName}");
				}
				else
				{
					//Evaluate the algorithm
					var evaluationResult = evaluator.Evaluate();

					//Add the algorithm name to each error message (there's not a better place to do it without a lot of work)
					for (int i = 0; i < evaluationResult.FailureMessages.Count; i++)
					{
						evaluationResult.FailureMessages[i] = $"{algo.AlgorithmName}: {evaluationResult.FailureMessages[i]}";
					}

					//Add this result to the results
					result.AlgorithmResults.Add(evaluationResult);
					//result.AlgorithmResults.Add(evaluator.Evaluate());
				}
			}

			return result;
		}

		public List<IAlgorithm> BuildAlgorithms(InfFile infFile)
		{
			//Build all the algorithms within this inf file
			List<IAlgorithm> algorithms = new List<IAlgorithm>();

			foreach (InfAlgorithm infAlgorithm in infFile.Algorithms)
			{
				//Build the algorithm object(s) - most return 1, some return multiple
				algorithms.AddRange(_algorithmFactory.GetAlgorithms(infAlgorithm));
			}

			return algorithms;
		}

		public List<AlgorithmPrerequisite> ExtractPrerequisites(List<IAlgorithm> algorithms)
		{
			//This cleans and merges all the prerequisites from all the algorithms, putting them in the proper format
			List<AlgorithmPrerequisite> prereqs = new List<AlgorithmPrerequisite>();


			//Trying to go from a collection of algorithms, each with a name and a collection of prerequisites to a collection of AlgorithmPrerequisites, each having a name and a collection of prerequisites
			//Needs to deal with there being multiple instances of each algorithm, and remove redundant prereqs within an algorithm

			//Loop through the distinct algorithm names
			foreach (string algorithmName in algorithms.Select(a => a.Algorithm).Distinct())
			{
				//Get the set of unique prerequisites for this algorithm - uses a custom comparer on the Distinct operator
				List<Prerequisite> prereqsForAlgo = algorithms.Where(a => a.Algorithm == algorithmName && a.CleanPreReqs != null).SelectMany(a => a.CleanPreReqs).Distinct(new PrerequisiteComparer()).ToList();

				//Create a prereq object if there are any
				if (prereqsForAlgo != null && prereqsForAlgo.Count > 0)
				{
					prereqs.Add(new AlgorithmPrerequisite
					{
						Algorithm = algorithmName,
						Prerequisites = prereqsForAlgo.ToList()
					});
				}
			}

			//Return the entire prerequisites collection or a null
			return prereqs.Count == 0 ? null : prereqs;
		}


		private OperationalEnvironment GetOE(InfFile infFile)
		{
			//Parse the OE out of the inf file. This will only have text in the objects, no URLs
			return ParseOperationalEnvironment(infFile);
		}


		public OperationalEnvironment ParseOperationalEnvironment(InfFile infFile)
		{
			OperationalEnvironment operationalEnvironment = new OperationalEnvironment();

			string operatingSystem = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "OE-operating system").Value);
			string processor = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "OE-processor").Value);

			if (string.IsNullOrWhiteSpace(operatingSystem))
			{
				//Just hardware/firmware, but in the case of a device they sometimes don't provide a processor, so need to do something funky
				if (string.IsNullOrWhiteSpace(processor))
				{
					//The weird case, a Device. Use the module name as the OE, because what else can we be sure is there to use?
					operationalEnvironment.Name = infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "implementation_name").Value;
					operationalEnvironment.Dependencies = new List<IDependency> { new Dependency { Type = "device",
																								  Name = infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "implementation_name").Value} };
				}
				else
				{
					//The normal case
					operationalEnvironment.Name = processor;
					operationalEnvironment.Dependencies = new List<IDependency> { new ProcessorDependency { Name = processor } };
				}

			}
			else
			{
				//It is software, so we have an OS, but don't know if we have a hardware dependency
				if (string.IsNullOrWhiteSpace(processor))
				{
					operationalEnvironment.Name = operatingSystem;
					operationalEnvironment.Dependencies = new List<IDependency> { new Dependency {   Type = "software",
																									Name = operatingSystem } };
				}
				else
				{
					operationalEnvironment.Name = $"{operatingSystem} on {processor}";
					operationalEnvironment.Dependencies = new List<IDependency> { new Dependency {   Type = "software",
																									Name = operatingSystem},
																				new ProcessorDependency { Name = processor }};

				}
			}

			return operationalEnvironment;
		}

		private string ParseSpecialInstructions(InfFile infFile)
		{
			return CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "other_requests_and_notes").Value);
		}

		private string CleanValue(string value)
		{
			//Trim, scrub default garbage, and make null if left with an empty string
			string cleanedValue = value.Trim().Replace("n/a", "").Replace("N/A", "");
			return string.IsNullOrEmpty(cleanedValue) ? null : cleanedValue;
		}


		private Module GetModule(string infFilePath)
		{
			//Get and parse the inf file
			InfFile infFile = _infFileParser.Parse(infFilePath);

			if (!infFile.Valid)
			{
				return null;
			}

			//Now we have an Inf file, so build a module object from it
			Module module = ParseModule(infFile);

			//Also need to build a vendor object
			module.Vendor = ParseVendor(infFile);

			//Also the Contacts
			module.Contacts = ParseContacts(infFile);

			return module;
		}

		public Module ParseModule(InfFile infFile)
		{
			//Create the base object with the simple fields
			Module module = new Module
			{
				Name = infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "implementation_name").Value,
				Description = infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "impl_description").Value
			};

			//Type and Version are a bit messy, having to consider combinations of 6 fields from the infFile
			if (infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "impl_type_sw").Value == "Yes")
			{
				module.Type = "Software";
				module.Version = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "soft_version").Value);
			}
			else
			{
				string fwVersion = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "firm_version").Value);

				if (infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "impl_type_hw").Value == "Yes")
				{
					module.Type = "Hardware";

					//Hardware may or may not need to include a firmware version, so concatenate a space and the value
					if (!string.IsNullOrEmpty(fwVersion)) fwVersion = " " + fwVersion;
					module.Version = $"{CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "part_number").Value)}{fwVersion}";
				}
				else if (infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "impl_type_fw").Value == "Yes")
				{
					module.Type = "Firmware";
					module.Version = fwVersion;
				}
				else
				{
					module.Type = "Unknown";
				}
			}

			return module;
		}

		public Vendor ParseVendor(InfFile infFile)
		{
			return new Vendor
			{
				Name = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "vendor_name").Value),
				Website = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "URL").Value),
				Address = new List<Address> { new Address
					{
						Street1 = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"address1").Value),
						Street2 = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"address2").Value),
						Street3 = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"address3").Value),
						Locality = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "city").Value),
						Region = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "state").Value),
						Country = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "country").Value),
						PostalCode = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == "zip code").Value)
					}
				}
			};
		}

		public List<Contact> ParseContacts(InfFile infFile)
		{
			//There will always be at least 1 contact created, I hope...
			List<Contact> contacts = new List<Contact>();

			//Loop through the 2 potential contacts, create and add them if they're valid
			for (int i = 1; i <= 2; i++)
			{
				//Grab all the values, and clean them of any garbage
				string name = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"contact{i}").Value);
				string email = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"contact{i}_email").Value);
				string phone = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"contact{i}_phone").Value);
				string fax = CleanValue(infFile.VendorAndImplementationSection.KeyValuePairs.First(k => k.Key == $"contact{i}_fax").Value);

				if (name != null || email != null || phone != null || fax != null)
				{
					Contact contact = new Contact
					{
						Name = name ?? ""       //Database doesn't allow nulls, so make it an empty string
					};

					if (email != null)
					{
						contact.Emails.Add(email);
					}

					if (phone != null)
					{
						contact.PhoneNumbers.Add(new PhoneNumber { Type = "voice", Number = phone });
					}

					if (fax != null)
					{
						contact.PhoneNumbers.Add(new PhoneNumber { Type = "fax", Number = fax });
					}

					contacts.Add(contact);
				}
			}

			return contacts;
		}


	}
}
