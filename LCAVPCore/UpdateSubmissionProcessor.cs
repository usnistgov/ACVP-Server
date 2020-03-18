using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmEvaluators;
using LCAVPCore.Registration;
using LCAVPCore.Registration.Algorithms;
using LCAVPCore.Registration.Algorithms.Component;
using LCAVPCore.Registration.Algorithms.ECDSA;
using Mighty;
using Newtonsoft.Json;

namespace LCAVPCore
{
	public class UpdateSubmissionProcessor : IUpdateSubmissionProcessor
	{
		private readonly IAlgorithmFactory _algorithmFactory;
		private readonly IAlgorithmEvaluatorFactory _algorithmEvaluatorFactory;
		private readonly IInfFileParser _infFileParser;
		private readonly IDataProvider _dataProvider;

		public UpdateSubmissionProcessor(IAlgorithmFactory algorithmFactory, IAlgorithmEvaluatorFactory algorithmEvaluatorFactory, IInfFileParser infFileParser, IDataProvider dataProvider)
		{
			_algorithmFactory = algorithmFactory;
			_algorithmEvaluatorFactory = algorithmEvaluatorFactory;
			_infFileParser = infFileParser;
			_dataProvider = dataProvider;
		}


		public List<ProcessingResult> Process(string submissionRoot)
		{
			List<ProcessingResult> results = new List<ProcessingResult>();

			//Get the update file Don't care if there are files in every directory for an update, only need one. Due to some weird ways they could package files, don't know which directory might be the first to have one, so search them all
			string updateFilePath = null;
			try
			{
				updateFilePath = Directory.EnumerateFiles(submissionRoot, "*Update_Req_File.txt", SearchOption.AllDirectories).FirstOrDefault();
			}
			catch
			{
				results.Add(new ProcessingResult
				{
					Type = ProcessingType.Update,
					Errors = new List<string> { "Unable to locate update file directory" }
				});
				return results;
			}

			if (string.IsNullOrEmpty(updateFilePath))
			{
				results.Add(new ProcessingResult
				{
					Type = ProcessingType.Update,
					Errors = new List<string> { "Unable to locate update file" }
				});
				return results;
			}

			//Parse the update file - no different than a change file
			ChangeFile changeFile = new ChangeFile(updateFilePath);
			if (!changeFile.Valid)
			{
				results.Add(new ProcessingResult { Type = ProcessingType.Update, Errors = changeFile.Errors });
				return results;
			}

			//Get the module ID to put in the resulting registrations
			int moduleID = _dataProvider.GetModuleID(changeFile);

			if (moduleID == -1)
			{
				results.Add(new ProcessingResult
				{
					Type = ProcessingType.Update,
					Errors = new List<string> { $"Unable to locate module record for {changeFile.ImplementationName}" }
				});
				return results;
			}

			//Parse out the collection of existing validations to be updated by this update. This will be used later to associate capabilities with the right registrations
			//This needs to be an aggregate of all update files in all the folders, as they could have split an OE into multiple folders
			//Get the distinct values, because if they used new style identifiers there could be multiple of the same thing
			List<(string Algorithm, int CertNumber)> affectedValidations = new List<(string Algorithm, int CertNumber)>();

			foreach (string folder in Directory.EnumerateDirectories(submissionRoot))
			{
				//Get the update file
				string updateFileName = null;
				updateFileName = Directory.EnumerateFiles(folder).SingleOrDefault(f => f.EndsWith("Update_Req_File.txt"));

				//If not a random folder without an update file, parse and extract the affected validations
				if (updateFileName != null)
				{
					ChangeFile updateFile = new ChangeFile(updateFileName);

					//Add any that are new
					foreach (var (Algorithm, CertNumber) in updateFile.AffectedValidations)
					{
						if (!affectedValidations.Exists(v => v.Algorithm == Algorithm && v.CertNumber == CertNumber))
						{
							affectedValidations.Add((Algorithm, CertNumber));
						}
					}
				}
			}

			//Verify (as best we can) that the validations referenced in the update file are against the product referenced in the update file - did they make a typo or forget the C on a C validation?
			List<string> productNames = _dataProvider.GetProductNameForValidations(affectedValidations);

			if (productNames.Count > 1)     //More than 1 name coming back means at least one is messed up
			{
				results.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Validations listed in the Update_Req_File are not all for the same product, so at least one validation is incorrect" } });
				return results;
			}
			else if (productNames.Count == 0)       //Not getting any is really weird
			{
				results.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Unable to get the product name for at least one validation referenced in a Update_Req_File. This is most commonly caused by a typo in the number or listing it under the wrong algorithm." } });
				return results;
			}
			else if (productNames[0] != changeFile.ImplementationName)  //Only found 1, which is good, but it didn't match the file. Most likely they were trying to reference C validations but forgot the C.
			{
				results.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Product name of referenced validations does not match the product name in the Update_Req_File. This is most commonly caused by trying to reference a C validation but omitting the C, or simply entering the wrong validation number, but may also be caused by an error in the product name." } });
				return results;
			}

			//Later we will (or at least may) break up the registrations that would naturally be generated by an inf file into multiple, so rather than creating new scenarios on a single registration for this submission, build a collection of what are effectively the results of each inf file
			List<(OperationalEnvironment OE, List<AlgorithmCapabilities> Algorithms)> rawInfRegistrationContent = new List<(OperationalEnvironment OE, List<AlgorithmCapabilities> Algorithms)>();

			//To handle errors better given the different way Updates produce their registrations, want to collect all the errors external to any processing results
			List<string> errors = new List<string>();

			//Process each of the inf files and build up the capabilities objects
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
							errors.AddRange(infEvaluationResult.Errors);
						}
						else
						{
							errors.AddRange(infEvaluationResult.Errors.Select(x => $"{infEvaluationResult.InfFile.FolderNumber} - {x}"));
						}
					}
					else
					{
						//if it was at least a valid inf file, then want to build the registration, whether or not it passed

						//Add any special instructions
						if (!string.IsNullOrEmpty(infEvaluationResult.SpecialInstructions))
						{
							errors.Add(infEvaluationResult.SpecialInstructions);
						}

						//Add any validation errors
						if (!infEvaluationResult.Success)
						{
							errors.AddRange(infEvaluationResult.AlgorithmResults.SelectMany(x => x.FailureMessages).Select(x => $"{infEvaluationResult.InfFile.FolderNumber} - {x}"));
						}

						//Parse the inf file into the different algorithms
						List<IAlgorithm> algorithms = BuildAlgorithms(infEvaluationResult.InfFile);

						//Add this OE and its collection of algorithms to the collection of raw results
						rawInfRegistrationContent.Add((GetOE(infEvaluationResult.InfFile), algorithms.Select(a => new AlgorithmCapabilities(a)).ToList()));
					}
				}
			}

			//If there were any errors, want to create a dummy processing result to capture them all, especially since in the case of a hard error (like bad file) there may not be any registrations generated
			if (errors.Count > 0)
			{
				results.Add(new ProcessingResult { Type = ProcessingType.Update, WorkflowType = WorkflowType.Validation, Errors = errors });
			}

			//Now have all the raw materials to turn into one or more registration, so need to figure out how to break up the inf file aligned results into validation record aligned results
			//This is a tricky process... There are a number of weird cases
			// - They are updating something that was issued as a C validation, so it gets everything
			// - They are updating old validations of things where we've changed the way the algos look, like ECDSA SigGen Component
			// - They are updating an old validation by adding something that falls under a family that it never did before, so in the old world it would have gotten a new family, but now needs a new C validation
			// - So really need to map between the inf content, the validation numbers provided, and potentially a new validation


			//First, try to assign each of the algorithms to existing old style validations

			//For anything left, if they have provided an A or C validation number, assign those algos to that
			//If they have not provided an A or C, then need to create a new validation for those algorithms

			//Need an algorithm collection to know which ones have been accounted for by old style validations
			List<int> algorithmsMappedToValidations = new List<int>();


			//Collection to store the registrations that we'll group and turn into one or more scenarios
			List<(OperationalEnvironment oe, List<AlgorithmCapabilities> algorithmCapabilities, string algorithmJson)> potentialScenarios = new List<(OperationalEnvironment oe, List<AlgorithmCapabilities> algorithmCapabilities, string algorithmJson)>();


			//First, try to assign each of the algorithms to existing old style validations
			foreach (var (AlgorithmFamily, CertNumber) in affectedValidations.Where(x => x.Algorithm != "A" && x.Algorithm != "C"))
			{
				potentialScenarios = new List<(OperationalEnvironment oe, List<AlgorithmCapabilities> algorithmCapabilities, string algorithmJson)>();

				//Figure out which validation this applies to
				int validationRecordID = _dataProvider.GetValidationRecordID(AlgorithmFamily, CertNumber);

				if (validationRecordID == -1)
				{
					errors.Add($"Unable to locate validation record for {AlgorithmFamily}-{CertNumber}");
				}

				//Create the object to serialize into the registration
				UpdateRegistrationContainer registration = new UpdateRegistrationContainer
				{
					ValidationID = validationRecordID,
					ModuleID = moduleID
				};


				//Need to get the algorithms that are specific to this algorithm family and generate a registration/processing result just for that

				//Have a few special cases - Component (which has a sub special case), and ECDSA
				switch (AlgorithmFamily)
				{
					case "ECDSA":
						foreach (var (OE, Algorithms) in rawInfRegistrationContent)
						{
							//Filter the algorithms down to ones that are members of the ECDSA family, but don't include SigGen if it is an instance of the component (which is just a flag in the capabilities)
							var applicableAlgorithms = Algorithms.Where(a => a.Algorithm.Family == AlgorithmFamily && (a.Algorithm.AlgorithmID != 69 || (a.Algorithm.AlgorithmID == 69 && a.Algorithm is ECDSA_SigGen))).ToList();

							if (applicableAlgorithms.Count > 0)
							{
								//registration.Scenarios.Add(new Scenario
								//{
								//	OEs = new List<OperationalEnvironment> { OE },
								//	Algorithms = applicableAlgorithms
								//});

								potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));

								//Add these algorithms to the list of ones we've mapped to a validation
								algorithmsMappedToValidations.AddRange(applicableAlgorithms.Select(x => x.Algorithm.AlgorithmID));
							}
						}


						//Group the OEs by distinct registration
						foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
						{
							//Get one of the algorithm collections to use for this scenario
							List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

							//Get all the OEs that have this same registration
							List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

							//Add the scenario
							registration.Scenarios.Add(new Scenario
							{
								Algorithms = algorithmCapabilities,
								OEs = oes
							});
						}



						break;

					case "Component":
						int[] algorithmIDs;
						//First get one of the algorithms in this validation, that will tell us which subset to get
						switch (GetAnAlgorithmOnValidation(validationRecordID))
						{
							case 69:
								//ECDSA SigGen - this is the special case
								algorithmIDs = new int[] { 69 };
								break;
							case 81:
								//RSA DP
								algorithmIDs = new int[] { 81 };
								break;
							case 82:
								//RSA SP
								algorithmIDs = new int[] { 82 };
								break;
							case 84:
							case 85:
							case 86:
								//KAS
								algorithmIDs = new int[] { 84, 85, 86 };
								break;
							case 73:
							case 74:
							case 75:
							case 76:
							case 77:
							case 78:
							case 79:
								//KDF 108
								algorithmIDs = new int[] { 73, 74, 75, 76, 77, 78, 79 };
								break;
							default:
								algorithmIDs = new int[] { -1 };
								break;
						}

						

						foreach (var (OE, Algorithms) in rawInfRegistrationContent)
						{
							//Filter the algorithms down to ones that belong in this grouping, but if ECDSA Sig Gen make sure we only take the component capabilities, not the regular ECDSA Sig Gen capabilities
							var applicableAlgorithms = Algorithms.Where(a => algorithmIDs.Contains(a.Algorithm.AlgorithmID) && (a.Algorithm.AlgorithmID != 69 || (a.Algorithm.AlgorithmID == 69 && a.Algorithm is ECDSASigGenComponent))).ToList();

							if (applicableAlgorithms.Count > 0)
							{
								//registration.Scenarios.Add(new Scenario
								//{
								//	OEs = new List<OperationalEnvironment> { OE },
								//	Algorithms = applicableAlgorithms
								//});

								potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));

								//Add these algorithms to the list of ones we've mapped to a validation
								algorithmsMappedToValidations.AddRange(applicableAlgorithms.Select(x => x.Algorithm.AlgorithmID));
							}
						}

						//Group the OEs by distinct registration
						foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
						{
							//Get one of the algorithm collections to use for this scenario
							List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

							//Get all the OEs that have this same registration
							List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

							//Add the scenario
							registration.Scenarios.Add(new Scenario
							{
								Algorithms = algorithmCapabilities,
								OEs = oes
							});
						}

						break;

					default:    //All the normal algorithm families
						foreach (var (OE, Algorithms) in rawInfRegistrationContent)
						{
							//Filter the algorithms down to ones that are members of this family
							var applicableAlgorithms = Algorithms.Where(a => a.Algorithm.Family == AlgorithmFamily).ToList();

							if (applicableAlgorithms.Count > 0)
							{
								//registration.Scenarios.Add(new Scenario
								//{
								//	OEs = new List<OperationalEnvironment> { OE },
								//	Algorithms = applicableAlgorithms
								//});

								potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));

								//Add these algorithms to the list of ones we've mapped to a validation
								algorithmsMappedToValidations.AddRange(applicableAlgorithms.Select(x => x.Algorithm.AlgorithmID));
							}
						}

						//Group the OEs by distinct registration
						foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
						{
							//Get one of the algorithm collections to use for this scenario
							List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

							//Get all the OEs that have this same registration
							List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

							//Add the scenario
							registration.Scenarios.Add(new Scenario
							{
								Algorithms = algorithmCapabilities,
								OEs = oes
							});
						}


						break;

				}



				//Check for a seemingly impossible case of there not being any algos in a referenced family - maybe they referenced AES because they wanted the metadata change to apply to it, but the capabilities changed only applies to TDES?
				if (registration.Scenarios.Count > 0)
				{
					//See if there were any "In this same implementation" prerequisites in the update, which the back end doesn't handle quite right
					//if (HasInThisSameImplPrereq(registration.Scenarios))
					//{
					//	errors.Add(@"Includes a ""In this same implementation"" prerequisite - figure out what they should really be and modify before pushing through, or push through and send the submission to Shane so he can clean it up");
					//}

					//Handle any "In this same implementation" prereqs
					registration.Scenarios = HandlePrereqLookups(registration.Scenarios, moduleID);

					if (registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Any(x => x.IsUnprocessedSubmission))
					{
						string submissions = string.Join(", ", registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Where(x => x.IsUnprocessedSubmission).Select(x => x.SubmissionID));
						errors.Add($"Cannot be processed because it depends on other submissions that have not been approved. Retry after you have approved submission(s) {submissions}");
					}

					//Create the processing result
					ProcessingResult processingResult = new ProcessingResult
					{
						Type = ProcessingType.Update,
						WorkflowType = WorkflowType.Validation,
						RegistrationJson = JsonConvert.SerializeObject(registration),       //Serialize the registration to JSON to include in the processing result
						TheThingy = registration,
						Errors = errors             //Putting all the errors on each registration, even if it was a failure in TDES testing and this is the AES registration
					};

					//Add the result to the collection
					results.Add(processingResult);
				}
			}


			//If there are any algorithms remaining that have not been mapped to an old style validation number, then need to put them in a C validation. Maybe they provided one, maybe they didn't...

			//Determine which algorithms were not part of any of the old style validations - what's left that needs to go on a C validation
			IEnumerable<int> unmappedAlgorithms = rawInfRegistrationContent.SelectMany(x => x.Algorithms).Select(x => x.Algorithm).Select(x => x.AlgorithmID).Except(algorithmsMappedToValidations);

			if (unmappedAlgorithms.Count() > 0)
			{
				potentialScenarios = new List<(OperationalEnvironment oe, List<AlgorithmCapabilities> algorithmCapabilities, string algorithmJson)>();

				int validationRecordID = -1;

				//Have at least one algorithm that needs to go into a C validation. Did they provide a C validation number, or do we have to create a new one
				if (affectedValidations.Exists(x => x.Algorithm == "C"))
				{
					//C validation number was provided, so get the record ID
					int certNumber = affectedValidations.First(x => x.Algorithm == "C").CertNumber;
					validationRecordID = _dataProvider.GetValidationRecordID("C", certNumber);

					if (validationRecordID <= 0)
					{
						errors.Add($"Unable to locate validation record for C-{certNumber}");
					}

					//Create an update registration
					UpdateRegistrationContainer registration = new UpdateRegistrationContainer
					{
						ValidationID = validationRecordID,
						ModuleID = moduleID
					};

					//Add the OE and algorithms to it
					foreach (var (OE, Algorithms) in rawInfRegistrationContent)
					{
						//Filter the algorithms down to the unmapped ones
						var applicableAlgorithms = Algorithms.Where(a => unmappedAlgorithms.Contains(a.Algorithm.AlgorithmID)).ToList();

						if (applicableAlgorithms.Count > 0)
						{
							//registration.Scenarios.Add(new Scenario
							//{
							//	OEs = new List<OperationalEnvironment> { OE },
							//	Algorithms = applicableAlgorithms
							//});
							potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));
						}
					}

					//Group the OEs by distinct registration
					foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
					{
						//Get one of the algorithm collections to use for this scenario
						List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

						//Get all the OEs that have this same registration
						List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

						//Add the scenario
						registration.Scenarios.Add(new Scenario
						{
							Algorithms = algorithmCapabilities,
							OEs = oes
						});
					}

					//See if there were any "In this same implementation" prerequisites in the update, which the back end doesn't handle quite right
					//if (HasInThisSameImplPrereq(registration.Scenarios))
					//{
					//	errors.Add(@"Includes a ""In this same implementation"" prerequisite - figure out what they should really be and modify before pushing through, or push through and send the submission to Shane so he can clean it up");
					//}

					//Handle any "In this same implementation" prereqs
					registration.Scenarios = HandlePrereqLookups(registration.Scenarios, moduleID);

					if (registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites ?? new List<Prerequisite>()).Any(x => x.IsUnprocessedSubmission))
					{
						string submissions = string.Join(", ", registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Where(x => x.IsUnprocessedSubmission).Select(x => x.SubmissionID));
						errors.Add($"Cannot be processed because it depends on other submissions that have not been approved. Retry after you have approved submission(s) {submissions}");
					}

					//Create the processing result from the registration
					ProcessingResult processingResult = new ProcessingResult
					{
						Type = ProcessingType.Update,
						WorkflowType = WorkflowType.Validation,
						RegistrationJson = JsonConvert.SerializeObject(registration),       //Serialize the registration to JSON to include in the processing result
						TheThingy = registration,
						Errors = errors         //Putting all the errors on each registration, even if it was a failure in TDES testing and this is the AES registration
					};

					//Add the result to the collection
					results.Add(processingResult);
				}
				else
				{
					//They didn't give us a C validation, but does one exist for this product and they just didn't give it to us?
					validationRecordID = _dataProvider.GetCValidationRecordIDForProduct(moduleID);

					if (validationRecordID > 0)
					{
						//Found the existing C validation, so update it
						UpdateRegistrationContainer registration = new UpdateRegistrationContainer
						{
							ValidationID = validationRecordID,
							ModuleID = moduleID
						};

						//Add the OE and algorithms to it
						foreach (var (OE, Algorithms) in rawInfRegistrationContent)
						{
							//Filter the algorithms down to ones that are members of this family
							var applicableAlgorithms = Algorithms.Where(a => unmappedAlgorithms.Contains(a.Algorithm.AlgorithmID)).ToList();

							if (applicableAlgorithms.Count > 0)
							{
								//registration.Scenarios.Add(new Scenario
								//{
								//	OEs = new List<OperationalEnvironment> { OE },
								//	Algorithms = applicableAlgorithms
								//});

								potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));
							}
						}

						//Group the OEs by distinct registration
						foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
						{
							//Get one of the algorithm collections to use for this scenario
							List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

							//Get all the OEs that have this same registration
							List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

							//Add the scenario
							registration.Scenarios.Add(new Scenario
							{
								Algorithms = algorithmCapabilities,
								OEs = oes
							});
						}

						//See if there were any "In this same implementation" prerequisites in the update, which the back end doesn't handle quite right
						//if (HasInThisSameImplPrereq(registration.Scenarios))
						//{
						//	errors.Add(@"Includes a ""In this same implementation"" prerequisite - figure out what they should really be and modify before pushing through, or push through and send the submission to Shane so he can clean it up");
						//}

						//Handle any "In this same implementation" prereqs
						registration.Scenarios = HandlePrereqLookups(registration.Scenarios, moduleID);

						if (registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Any(x => x.IsUnprocessedSubmission))
						{
							string submissions = string.Join(", ", registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Where(x => x.IsUnprocessedSubmission).Select(x => x.SubmissionID));
							errors.Add($"Cannot be processed because it depends on other submissions that have not been approved. Retry after you have approved submission(s) {submissions}");
						}

						//Create the processing result from the registration
						ProcessingResult processingResult = new ProcessingResult
						{
							Type = ProcessingType.Update,
							WorkflowType = WorkflowType.Validation,
							RegistrationJson = JsonConvert.SerializeObject(registration),       //Serialize the registration to JSON to include in the processing result
							TheThingy = registration,
							Errors = errors         //Putting all the errors on each registration, even if it was a failure in TDES testing and this is the AES registration
						};

						//Add the result to the collection (which will now only have a single member, but still...)
						results.Add(processingResult);

					}
					else
					{
						//Need to do a new registration to get a new C validation
						//Create new registration
						NewRegistrationContainer registration = new NewRegistrationContainer
						{
							Module = new Module { ID = moduleID } //Since we know the existing product, just use that ID, otherwise a new product will be created
						};

						//Add the OE and algorithms to it
						foreach (var (OE, Algorithms) in rawInfRegistrationContent)
						{
							//Filter the algorithms down to ones that are members of this family
							var applicableAlgorithms = Algorithms.Where(a => unmappedAlgorithms.Contains(a.Algorithm.AlgorithmID)).ToList();

							if (applicableAlgorithms.Count > 0)
							{
								//registration.Scenarios.Add(new Scenario
								//{
								//	OEs = new List<OperationalEnvironment> { OE },
								//	Algorithms = applicableAlgorithms
								//});

								potentialScenarios.Add((OE, applicableAlgorithms, JsonConvert.SerializeObject(applicableAlgorithms)));
							}
						}

						//Group the OEs by distinct registration
						foreach (string registrationJson in potentialScenarios.Select(x => x.algorithmJson).Distinct().ToList())
						{
							//Get one of the algorithm collections to use for this scenario
							List<AlgorithmCapabilities> algorithmCapabilities = potentialScenarios.First(x => x.algorithmJson == registrationJson).algorithmCapabilities;

							//Get all the OEs that have this same registration
							List<OperationalEnvironment> oes = potentialScenarios.Where(x => x.algorithmJson == registrationJson).Select(x => x.oe).ToList();

							//Add the scenario
							registration.Scenarios.Add(new Scenario
							{
								Algorithms = algorithmCapabilities,
								OEs = oes
							});
						}

						//See if there were any "In this same implementation" prerequisites in the update, which the back end doesn't handle quite right
						//if (HasInThisSameImplPrereq(registration.Scenarios))
						//{
						//	errors.Add(@"Includes a ""In this same implementation"" prerequisite - figure out what they should really be and modify before pushing through, or push through and send the submission to Shane so he can clean it up");
						//}

						//Handle any "In this same implementation" prereqs
						registration.Scenarios = HandlePrereqLookups(registration.Scenarios, moduleID);

						if (registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Any(x => x.IsUnprocessedSubmission))
						{
							string submissions = string.Join(", ", registration.Scenarios.SelectMany(x => x.Algorithms).SelectMany(x => x.Prerequisites).Where(x => x.IsUnprocessedSubmission).Select(x => x.SubmissionID));
							errors.Add($"Cannot be processed because it depends on other submissions that have not been approved. Retry after you have approved submission(s) {submissions}");
						}

						//Create processing result from the registration
						ProcessingResult processingResult = new ProcessingResult
						{
							Type = ProcessingType.New,
							WorkflowType = WorkflowType.Validation,
							RegistrationJson = JsonConvert.SerializeObject(registration),       //Serialize the registration to JSON to include in the processing result
							TheThingy = registration,
							Errors = errors      //Putting all the errors on each registration, even if it was a failure in TDES testing and this is the AES registration
						};

						//Add the result to the collection
						results.Add(processingResult);
					}
				}




			}
			
			return results;
		}


		private bool HasInThisSameImplPrereq(List<Scenario> scenarios)
		{
			//This should be done via LINQ but I couldn't quite get it right, so did it partially old school. If there is a prereq that has no validation record id then it came from a "In this same implementation" - stop as soon as we hit one
			foreach (var scenario in scenarios)
			{
				foreach (var scenarioAlgorithm in scenario.Algorithms)
				{
					if (scenarioAlgorithm.Prerequisites != null && scenarioAlgorithm.Prerequisites.Exists(x => x.SubmissionID == null && x.ValidationRecordID == null)) { return true; }
				}
			}

			return false;
		}


		private List<Scenario> HandlePrereqLookups(List<Scenario> scenarios, int moduleID)
		{
			for (int i = 0; i < scenarios.Count; i++)
			{
				for (int j = 0; j < scenarios[i].Algorithms.Count; j++)
				{
					if (scenarios[i].Algorithms[j].Prerequisites != null)
					{
						for (int k = 0; k < scenarios[i].Algorithms[j].Prerequisites.Count; k++)
						{
							if (scenarios[i].Algorithms[j].Prerequisites[k].SubmissionID == null
								&& scenarios[i].Algorithms[j].Prerequisites[k].ValidationRecordID == null)
							{
								//Is a self referential prereq, so look up what that ID is - that may be complex since old stuff could be multiple validations
								int validationRecordID = _dataProvider.GetValidationRecordIDForModuleAndAlgo(moduleID, scenarios[i].Algorithms[j].Prerequisites[k].Algorithm);

								if (validationRecordID > 0)
								{
									scenarios[i].Algorithms[j].Prerequisites[k].ValidationRecordID = validationRecordID;
								}
							}
							else if (scenarios[i].Algorithms[j].Prerequisites[k].SubmissionID != null)
							{
								//References another submission, so look it up
								long validationID = _dataProvider.GetValidationIDForSubmissionID(scenarios[i].Algorithms[j].Prerequisites[k].SubmissionID);

								if (validationID == -1)
								{
									scenarios[i].Algorithms[j].Prerequisites[k].IsUnprocessedSubmission = true;
								}
								else
								{
									scenarios[i].Algorithms[j].Prerequisites[k].ValidationRecordID = validationID;
								}
							}
						}
					}
				}
			}

			return scenarios;
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
																									Name = operatingSystem} };
				}
				else
				{
					operationalEnvironment.Name = $"{operatingSystem} on {processor}";
					operationalEnvironment.Dependencies = new List<IDependency> { new Dependency {   Type = "software",
																									Name = operatingSystem},
																				new ProcessorDependency {    Type = "processor",
																									Name = processor}};

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


		private long GetAnAlgorithmOnValidation(long validationRecordID)
		{

			try
			{
				var db = new MightyOrm("LCAVP");

				var data = db.Query("EXEC [lcavp].[AlgorithmsOnValidationGet] @0", validationRecordID).FirstOrDefault();

				return (long)data.algorithm_id;
			}
			catch
			{
				return -1;
			}
		}

	}
}
