using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCAVPCore.Registration;
using Newtonsoft.Json;

namespace LCAVPCore
{
	public class ChangeSubmissionProcessor : IChangeSubmissionProcessor
	{
		private IDataProvider _dataProvider;

		public ChangeSubmissionProcessor(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}

		public List<ProcessingResult> Process(string submissionRoot)
		{
			//Need to keep a collection of results collections, one for each file
			List<List<ProcessingResult>> allFilesResults = new List<List<ProcessingResult>>();

			foreach (string folder in Directory.EnumerateDirectories(submissionRoot))
			{
				List<ProcessingResult> changeFileResults = new List<ProcessingResult>();
				//Get the change file
				string changeFilePath = null;
				try
				{
					changeFilePath = Directory.EnumerateFiles(folder, "*_Req_File.txt").FirstOrDefault();
				}
				catch
				{
					//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, "Unable to locate change file"));
					changeFileResults.Add(new ProcessingResult
					{
						Type = ProcessingType.Change,
						Errors = new List<string> { "Unable to locate change file" }
					});
					allFilesResults.Add(changeFileResults);
				}

				if (!string.IsNullOrEmpty(changeFilePath))
				{
					//Open/parse it
					ChangeFile changeFile = new ChangeFile(changeFilePath);
					if (!changeFile.Valid)
					{
						changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = changeFile.Errors });
					}
					else
					{
						//If the file actually contains changes (it might not if an update that only added capabilities), process the changes
						if (changeFile.IncludesChange)
						{
							//Try to verify that all the validations listed actually belong to this product - they didn't mean to include a C prefix but forgot, or made a typo
							List<string> productNames = _dataProvider.GetProductNameForValidations(changeFile.AffectedValidations.Distinct().ToList());

							if (productNames.Count > 1)     //More than 1 name coming back means at least one is messed up
							{
								changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Validations listed in the Update_Req_File/Change_Req_File are not all for the same product, so at least one is incorrect" } });
							}
							else if (productNames.Count == 0)       //Not getting any is really weird
							{
								changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Unable to get the product name for at least one validation referenced in a Update_Req_File/Change_Req_File. This is most commonly caused by a typo in the number or listing it under the wrong algorithm." } });
							}
							else if (productNames[0] != changeFile.ImplementationName)  //Only found 1, which is good, but it didn't match the file. Most likely they were trying to reference C validations but forgot the C.
							{
								changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, Errors = new List<string> { "Product name of referenced validations does not match the product name in the Update_Req_File/Change_Req_File. This is most commonly caused by trying to reference a C validation but omitting the C, or simply entering the wrong validation number, but may also be caused by an error in the product name." } });
							}
							else
							{
								//Handle Vendor/Address changes
								if (changeFile.IncludesVendorChange || changeFile.IncludesAddressChange)
								{
									(Vendor vendor, string errorMessage) = BuildVendorUpdate(changeFile);

									if (!String.IsNullOrEmpty(errorMessage))
									{
										//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Organization, errorMessage));
										changeFileResults.Add(new ProcessingResult
										{
											Type = ProcessingType.Change,
											WorkflowType = WorkflowType.Organization,
											Errors = new List<string> { errorMessage }
										});
									}
									else
									{
										changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, WorkflowType = WorkflowType.Organization, RegistrationJson = JsonConvert.SerializeObject(vendor), TheThingy = vendor });
									}
								}

								//Handle contact changes - not additions
								if (changeFile.IncludesContact1Change && !changeFile.IncludesContact1Add)
								{
									(Contact contact1, List<string> errorMessages) = BuildContactUpdate(changeFile, 1);

									if (errorMessages.Count != 0)
									{
										//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Person, errorMessage));
										changeFileResults.Add(new ProcessingResult
										{
											Type = ProcessingType.Change,
											WorkflowType = WorkflowType.Person,
											Errors = errorMessages
										});
									}
									else
									{
										changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, WorkflowType = WorkflowType.Person, RegistrationJson = JsonConvert.SerializeObject(contact1), TheThingy = contact1 });
									}
								}

								if (changeFile.IncludesContact2Change && !changeFile.IncludesContact2Add)
								{
									(Contact contact2, List<string> errorMessages) = BuildContactUpdate(changeFile, 2);

									if (errorMessages.Count != 0)
									{
										//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Person, errorMessage));
										changeFileResults.Add(new ProcessingResult
										{
											Type = ProcessingType.Change,
											WorkflowType = WorkflowType.Person,
											Errors = errorMessages
										});
									}
									else
									{
										changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, WorkflowType = WorkflowType.Person, RegistrationJson = JsonConvert.SerializeObject(contact2), TheThingy = contact2 });
									}
								}

								//Handle Module changes
								if (changeFile.IncludesModuleChange || changeFile.IncludesContact1Add || changeFile.IncludesContact2Add)	// || changeFile.IncludesAddressChange)
								{
									(Module module, string errorMessage) = BuildModuleUpdate(changeFile);

									if (!String.IsNullOrEmpty(errorMessage))
									{
										//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Module, errorMessage));
										changeFileResults.Add(new ProcessingResult
										{
											Type = ProcessingType.Change,
											WorkflowType = WorkflowType.Module,
											Errors = new List<string> { errorMessage }
										});
									}
									else
									{
										changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, WorkflowType = WorkflowType.Module, RegistrationJson = JsonConvert.SerializeObject(module), TheThingy = module });
									}
								}

								//Handle OE changes
								if (changeFile.IncludesOEChange)
								{
									List<(OperationalEnvironment OperationalEnvironment, string ErrorMessage)> oeUpdates = BuildOEUpdate(changeFile);

									foreach ((OperationalEnvironment OperationalEnvironment, string ErrorMessage) in oeUpdates)
									{
										if (!String.IsNullOrEmpty(ErrorMessage))
										{
											//changeFileResults.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.OE, ErrorMessage));
											changeFileResults.Add(new ProcessingResult
											{
												Type = ProcessingType.Change,
												WorkflowType = WorkflowType.OE,
												Errors = new List<string> { ErrorMessage }
											});
										}
										else
										{
											changeFileResults.Add(new ProcessingResult { Type = ProcessingType.Change, WorkflowType = WorkflowType.OE, RegistrationJson = JsonConvert.SerializeObject(OperationalEnvironment), TheThingy = OperationalEnvironment });
										}
									}
								}
							}
						}
					}

					allFilesResults.Add(changeFileResults);
				}
			}

			//If only 1 change file (the usual, unless mulitple OEs, just return the first set as the complete set
			if (allFilesResults.Count == 1)
			{
				return allFilesResults[0];
			}
			else
			{
				//Have multiple change files, hopefully for multiple OEs, so make sure all the other changes are identical in all files.
				List<ProcessingResult> results = new List<ProcessingResult>();

				//Since each file should have the same changes other than the OE, that would produce multiple copies of the same org/module/person change, so just get the distinct changes
				IEnumerable<ProcessingResult> distinctResults = allFilesResults.SelectMany(x => x).Distinct(new ProcessingResultComparer());

				//Throw extra errors if there are multiple results for types there can only be 1 of (org, module)
				if (distinctResults.Where(x => x.WorkflowType == WorkflowType.Module).Count() > 1)
				{
					//results.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Module, "Multiple change files found containing inconsistent changes to the Module data"));
					results.Add(new ProcessingResult
					{
						Type = ProcessingType.Change,
						WorkflowType = WorkflowType.Module,
						Errors = new List<string> { "Multiple change files found containing inconsistent changes to the Module data" }
					});
				}

				if (distinctResults.Where(x => x.WorkflowType == WorkflowType.Organization).Count() > 1)
				{
					//results.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Module, "Multiple change files found containing inconsistent changes to the Organization data"));
					results.Add(new ProcessingResult
					{
						Type = ProcessingType.Change,
						WorkflowType = WorkflowType.Organization,
						Errors = new List<string> { "Multiple change files found containing inconsistent changes to the Organization data" }
					});
				}

				//Person changes can actually have 2, since there can be 2 contacts. While finding 2 could actually be incompatible changes to the same POC, we're going to go with this for simplicity
				if (distinctResults.Where(x => x.WorkflowType == WorkflowType.Person).Count() > 2)
				{
					//results.Add(new ProcessingResult(ProcessingType.Change, WorkflowType.Module, "Multiple change files found containing inconsistent changes to the Person data"));
					results.Add(new ProcessingResult
					{
						Type = ProcessingType.Change,
						WorkflowType = WorkflowType.Person,
						Errors = new List<string> { "Multiple change files found containing inconsistent changes to the Person data" }
					});
				}

				//Add whatever the distinct results were
				results.AddRange(distinctResults);

				return results;
			}
		}

		private string CleanValue(string value)
		{
			//Trim, scrub default garbage, and make null if left with an empty string
			string cleanedValue = value?.Trim().Replace("n/a", "").Replace("N/A", ""); ;
			return string.IsNullOrEmpty(cleanedValue) ? null : cleanedValue;
		}

		public (Vendor Vendor, string ErrorMessage) BuildVendorUpdate(ChangeFile changeFile)
		{
			//Get the current vendor ID
			int vendorID = _dataProvider.GetVendorID(changeFile);

			if (vendorID == -1)
			{
				return (null, "Unable to locate vendor record");
			}
			else
			{
				//Create a vendor object for ID with only the changed fields listed
				Vendor vendor = new Vendor { ID = vendorID };
				if (changeFile.UpdatedFields.ContainsKey("VendorName")) { vendor.Name = changeFile.UpdatedFields["VendorName"].UpdatedValue; vendor.NameUpdated = true; }
				if (changeFile.UpdatedFields.ContainsKey("WebSite")) { vendor.Website = changeFile.UpdatedFields["WebSite"].UpdatedValue; vendor.WebsiteUpdated = true; }

				if (changeFile.IncludesAddressChange)
				{
					(Address Address, string ErrorMessage) = BuildAddressUpdate(vendorID, changeFile);
					if (Address == null)
					{
						return (null, ErrorMessage);
					}
					else
					{
						vendor.Address = new List<Address> { Address };
						vendor.AddressUpdated = true;
					}
				}

				return (vendor, null);
			}
		}

		private (Address Address, string ErrorMessage) BuildAddressUpdate(int vendorID, ChangeFile changeFile)
		{
			//Get the current address ID
			int addressID = _dataProvider.GetAddressID(vendorID, changeFile);

			if (addressID == -1)
			{
				return (null, "Unable to locate address record");
			}
			else
			{
				Address address = new Address() { ID = addressID, VendorID = vendorID };

				if (changeFile.UpdatedFields.ContainsKey("Address1")) address.Street1 = CleanValue(changeFile.UpdatedFields.GetValue("Address1").UpdatedValue);
				if (changeFile.UpdatedFields.ContainsKey("Address2")) address.Street2 = CleanValue(changeFile.UpdatedFields.GetValue("Address2").UpdatedValue);
				if (changeFile.UpdatedFields.ContainsKey("Address3")) address.Street3 = CleanValue(changeFile.UpdatedFields.GetValue("Address3").UpdatedValue);
				if (changeFile.UpdatedFields.ContainsKey("City")) address.Locality = changeFile.UpdatedFields.GetValue("City").UpdatedValue;
				if (changeFile.UpdatedFields.ContainsKey("State")) address.Region = changeFile.UpdatedFields.GetValue("State").UpdatedValue;
				if (changeFile.UpdatedFields.ContainsKey("Zip")) address.PostalCode = changeFile.UpdatedFields.GetValue("Zip").UpdatedValue;
				if (changeFile.UpdatedFields.ContainsKey("Country")) address.Country = changeFile.UpdatedFields.GetValue("Country").UpdatedValue;

				return (address, null);
			}
		}

		public (Contact Contact, List<string> ErrorMessages) BuildContactUpdate(ChangeFile changeFile, int orderIndex)
		{
			List<string> errorMessages = new List<string>();

			//Get the ID of the person that is this contact
			int personID = _dataProvider.GetPersonIDFromContact(changeFile, orderIndex);

			if (personID == -1)
			{
				errorMessages.Add($"Unable to locate person record for contact #{orderIndex} referenced in the change file");
				return (null, errorMessages);
			}
			else
			{
				Contact contact = new Contact();
				contact.PersonID = personID;

				//Account for the small difference in the key names in the change file - has a 2 for contact 2, nothing for contact 1
				string contactNumberSuffix = orderIndex == 1 ? "" : "2";

				if (changeFile.UpdatedFields.ContainsKey($"Contact{contactNumberSuffix}Name")) contact.Name = changeFile.UpdatedFields[$"Contact{contactNumberSuffix}Name"].UpdatedValue ?? "";     //DB does not allow null, this would be null if they said N/A, so make it empty string
				if (changeFile.UpdatedFields.ContainsKey($"Email{contactNumberSuffix}")) contact.Emails.Add(changeFile.UpdatedFields[$"Email{contactNumberSuffix}"].UpdatedValue);

				//If either phone number is changed, need to provide the entire set - so need to get them from the database and build the new set
				if (changeFile.UpdatedFields.ContainsKey($"PhoneNo{contactNumberSuffix}") || changeFile.UpdatedFields.ContainsKey($"FAXNo{contactNumberSuffix}"))
				{
					//Explicitly set this new property, as otherwise there's no way to tell the difference between deleting all the phone numbers and not touching the phone numbers
					contact.PhoneNumbersUpdated = true;

					//Get the current phone numbers
					List<(int OrderIndex, string PhoneNumber, string Type)> phoneNumbers = _dataProvider.GetPhoneNumbersForPerson(personID);

					//Throw an error if the current number provided isn't actually one of the current numbers (if it isn't an add)
					if (changeFile.UpdatedFields.ContainsKey($"PhoneNo{contactNumberSuffix}") && !string.IsNullOrEmpty(changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].CurrentValue) && !phoneNumbers.Exists(p => p.Type == "voice" && p.PhoneNumber == changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].CurrentValue))
					{
						errorMessages.Add($"Tried to update voice number {changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].CurrentValue}, but that is not a current voice number for this person");
					}

					if (changeFile.UpdatedFields.ContainsKey($"FAXNo{contactNumberSuffix}") && !string.IsNullOrEmpty(changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].CurrentValue) && !phoneNumbers.Exists(p => p.Type == "fax" && p.PhoneNumber == changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].CurrentValue))
					{
						errorMessages.Add($"Tried to update fax number {changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].CurrentValue}, but that is not a current fax number for this person");
					}

					//First loop through the current numbers and make any changes - this includes changing to blank (delete), which we'll handle later
					for (int i = 0; i < phoneNumbers.Count; i++)
					{
						var phoneNumber = phoneNumbers[i];

						if (phoneNumber.Type == "voice" && changeFile.UpdatedFields.ContainsKey($"PhoneNo{contactNumberSuffix}") && phoneNumber.PhoneNumber == changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].CurrentValue)
						{
							phoneNumbers[i] = (phoneNumber.OrderIndex, changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].UpdatedValue, "voice");
						}

						if (phoneNumber.Type == "fax" && changeFile.UpdatedFields.ContainsKey($"FAXNo{contactNumberSuffix}") && phoneNumber.PhoneNumber == changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].CurrentValue)
						{
							phoneNumbers[i] = (phoneNumber.OrderIndex, changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].UpdatedValue, "fax");
						}
					}

					//Now do any additions - this means the current value in the change file is blank
					if (changeFile.UpdatedFields.ContainsKey($"PhoneNo{contactNumberSuffix}") && string.IsNullOrEmpty(changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].CurrentValue ))
					{
						phoneNumbers.Add((999, changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].UpdatedValue, "voice"));
					}

					if (changeFile.UpdatedFields.ContainsKey($"FAXNo{contactNumberSuffix}") && string.IsNullOrEmpty(changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].CurrentValue))
					{
						phoneNumbers.Add((999, changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].UpdatedValue, "fax"));
					}

					//Have the superset of phone numbers now (may include deletes). So add to the contact.PhoneNumbers all the ones we don't want to delete
					contact.PhoneNumbers.AddRange(phoneNumbers.Where(p => !string.IsNullOrEmpty(p.PhoneNumber)).Select(p => new PhoneNumber { Type = p.Type, Number = p.PhoneNumber }));
				}

				return (contact, errorMessages);
			}
		}

		private Contact BuildContactAdd(ChangeFile changeFile, int orderIndex, int vendorID)
		{
			//In an update need to provide the organization id within the contact

			string contactNumberSuffix = orderIndex == 1 ? "" : "2";
			Contact contact = new Contact
			{
				Name = changeFile.UpdatedFields[$"Contact{contactNumberSuffix}Name"].UpdatedValue ?? "",        //DB does not allow null, this would be null if they said N/A, so make it empty string,
				OrganizationID = vendorID
			};

			if (changeFile.UpdatedFields.ContainsKey($"Email{contactNumberSuffix}")) contact.Emails.Add(changeFile.UpdatedFields[$"Email{contactNumberSuffix}"].UpdatedValue);
			if (changeFile.UpdatedFields.ContainsKey($"PhoneNo{contactNumberSuffix}")) contact.PhoneNumbers.Add(new PhoneNumber { Type = "voice", Number = changeFile.UpdatedFields[$"PhoneNo{contactNumberSuffix}"].UpdatedValue });
			if (changeFile.UpdatedFields.ContainsKey($"FAXNo{contactNumberSuffix}")) contact.PhoneNumbers.Add(new PhoneNumber { Type = "fax", Number = changeFile.UpdatedFields[$"FAXNo{contactNumberSuffix}"].UpdatedValue });

			return contact;
		}

		public (Module Module, string ErrorMessage) BuildModuleUpdate(ChangeFile changeFile)
		{
			//Get the current module ID
			int moduleID = _dataProvider.GetModuleID(changeFile);

			if (moduleID == -1)
			{
				return (null, $"Unable to locate module record for {changeFile.ImplementationName}");
			}
			else
			{
				Module module = new Module { ID = moduleID };

				//Update the simple fields on the object
				if (changeFile.UpdatedFields.ContainsKey("ImplName")) { module.Name = changeFile.UpdatedFields.GetValue("ImplName").UpdatedValue; module.NameUpdated = true; }
				if (changeFile.UpdatedFields.ContainsKey("ImplDesc")) { module.Description = changeFile.UpdatedFields.GetValue("ImplDesc").UpdatedValue; module.DescriptionUpdated = true; }

				//The Type/Version data is full replacement, so can redo entirely based on the new values
				if (changeFile.IncludesTypeVersionChange)
				{
					//Determine if they actually changed the type
					bool typeChanged = changeFile.UpdatedFields.GetValue("Software").UpdatedValue != changeFile.UpdatedFields.GetValue("Software").CurrentValue
									|| changeFile.UpdatedFields.GetValue("Hardware").UpdatedValue != changeFile.UpdatedFields.GetValue("Hardware").CurrentValue
									|| changeFile.UpdatedFields.GetValue("Firmware").UpdatedValue != changeFile.UpdatedFields.GetValue("Firmware").CurrentValue;

					//Determine if they actually changed the version
					bool versionChanged = changeFile.UpdatedFields.GetValue("SoftVersion").UpdatedValue != changeFile.UpdatedFields.GetValue("SoftVersion").CurrentValue
									   || changeFile.UpdatedFields.GetValue("PartNo").UpdatedValue != changeFile.UpdatedFields.GetValue("PartNo").CurrentValue
									   || changeFile.UpdatedFields.GetValue("FirmVersion").UpdatedValue != changeFile.UpdatedFields.GetValue("FirmVersion").CurrentValue;

					module.TypeUpdated = typeChanged;
					module.VersionUpdated = versionChanged;

					//Type and Version are a bit messy, having to consider combinations of 6 fields
					if (changeFile.UpdatedFields.GetValue("Software").UpdatedValue == "Yes")
					{
						if (typeChanged) module.Type = "Software";
						if (versionChanged) module.Version = CleanValue(changeFile.UpdatedFields.GetValue("SoftVersion").UpdatedValue);
					}
					else
					{
						string fwVersion = CleanValue(changeFile.UpdatedFields.GetValue("FirmVersion").UpdatedValue);

						if (changeFile.UpdatedFields.GetValue("Hardware").UpdatedValue == "Yes")
						{
							if (typeChanged) module.Type = "Hardware";

							//Hardware may or may not need to include a firmware version, so concatenate a space and the value
							if (versionChanged)
							{
								if (!string.IsNullOrEmpty(fwVersion)) fwVersion = " " + fwVersion;
								module.Version = $"{changeFile.UpdatedFields.GetValue("PartNo").UpdatedValue}{fwVersion}";
							}
						}
						else if (changeFile.UpdatedFields.GetValue("Firmware").UpdatedValue == "Yes")
						{
							if (typeChanged) module.Type = "Firmware";
							if (versionChanged) module.Version = fwVersion;
						}
						else
						{
							module.Type = "Unknown";
							module.Version = null;
						}
					}
				}

				if (changeFile.IncludesContact1Add || changeFile.IncludesContact2Add)
				{
					int vendorID = _dataProvider.GetVendorID(changeFile);

					if (vendorID == -1)
					{
						return (null, "Unable to locate vendor record");
					}
					else
					{
						//Populate any contact additions - not updates
						if (changeFile.IncludesContact1Add)
						{
							module.Contact1Added = true;
							module.Contacts.Add(BuildContactAdd(changeFile, 1, vendorID));
						}

						if (changeFile.IncludesContact2Add)
						{
							module.Contact2Added = true;
							module.Contacts.Add(BuildContactAdd(changeFile, 2, vendorID));
						}
					}
				}

				//Populate any address changes
				if (changeFile.IncludesAddressChange)
				{
					//Need to get the vendor ID to look up the address - even though could get the address id from the module, still need the vendor id, so then would have to get the address record anyways
					int vendorID = _dataProvider.GetVendorID(changeFile);

					if (vendorID == -1)
					{
						return (null, "Unable to locate vendor record");
					}
					else
					{
						(Address Address, string ErrorMessage) = BuildAddressUpdate(vendorID, changeFile);
						if (Address == null)
						{
							return (null, ErrorMessage);
						}
						else
						{
							module.Address = Address;
						}
					}
				}

				return (module, null);
			}
		}

		public List<(OperationalEnvironment OperationalEnvironment, string ErrorMessage)> BuildOEUpdate(ChangeFile changeFile)
		{
			List<(OperationalEnvironment OperationalEnvironment, string ErrorMessage)> oes = new List<(OperationalEnvironment OperationalEnvironment, string ErrorMessage)>();

			//There are a few ways that we can wind up with multiple OEs with the same name in a submission, so may need to update multiple OEs in the database even though they're all supposed to represent the same thing

			//Get the current OE IDs
			List<int> oeIDs = _dataProvider.GetOEIDs(changeFile);

			if (oeIDs.Count == 0)
			{
				oes.Add((null, $"Unable to locate OE record for {changeFile.ImplementationName}"));
			}
			else
			{
				string oeName = "";
				string partDelimiter = "";  //If have OS and processor, need to put an " on " between them, but not if only one or the other

				//This assumes always creating new dependencies
				List<IDependency> dependencies = new List<IDependency>();

				//Add the OS dependency if needed
				string newOperatingSystem = changeFile.UpdatedFields["_operatingsystem"].UpdatedValue;
				if (!string.IsNullOrEmpty(newOperatingSystem))
				{
					dependencies.Add(new Dependency { Type = "software", Name = newOperatingSystem });
					oeName = newOperatingSystem;
					partDelimiter = " on ";
				}

				//Add the processor dependency if needed
				string newProcessor = changeFile.UpdatedFields["_processor"].UpdatedValue;
				if (!string.IsNullOrEmpty(newProcessor))
				{
					dependencies.Add(new ProcessorDependency { Name = newProcessor });

					oeName += partDelimiter + newProcessor;
				}

				foreach (int oeID in oeIDs)
				{
					oes.Add((new OperationalEnvironment
					{
						ID = oeID,
						Name = oeName,
						Dependencies = dependencies
					}, null));
				}
			}

			return oes;
		}
	}
}
