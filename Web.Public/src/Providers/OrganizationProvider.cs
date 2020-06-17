using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Web.Public.Helpers;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class OrganizationProvider : IOrganizationProvider
	{
		private readonly ILogger<OrganizationProvider> _logger;
		private readonly string _acvpConnectionString;

		public OrganizationProvider(IConnectionStringFactory connectionStringFactory, ILogger<OrganizationProvider> logger)
		{
			_logger = logger;
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public Organization Get(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var orgData = db.SingleFromProcedure("val.OrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				if (orgData == null)
					return null;

				var result = new Organization
				{
					ID = orgData.Id,
					Name = orgData.Name,
					Website = orgData.Website,
				};

				var phoneNumbers = new List<PhoneNumber>();
				if (!string.IsNullOrEmpty(orgData.VoiceNumber))
				{
					phoneNumbers.Add(new PhoneNumber { Number = orgData.VoiceNumber, Type = "voice" });
				}

				if (!string.IsNullOrEmpty(orgData.FaxNumber))
				{
					phoneNumbers.Add(new PhoneNumber { Number = orgData.FaxNumber, Type = "fax" });
				}

				if (phoneNumbers.Count > 0)
				{
					result.PhoneNumbers = phoneNumbers;
				}

				var addressData = db.QueryFromProcedure("val.AddressesForOrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				if (addressData != null)
				{
					result.Addresses = new List<Address>();

					foreach (var address in addressData)
					{
						result.Addresses.Add(new Address
						{
							ID = address.ID,
							OrganizationID = organizationID,
							Street1 = address.Street1,
							Street2 = address.Street2,
							Street3 = address.Street3,
							Locality = address.Locality,
							Region = address.Region,
							Country = address.Country,
							PostalCode = address.PostalCode
						});
					}
				}

				var emailData = db.QueryFromProcedure("val.OrganizationEmailsGet", inParams: new
				{
					OrganizationID = organizationID
				});

				if (emailData != null)
				{
					result.Emails = new List<string>();

					foreach (var email in emailData)
					{
						result.Emails.Add(email.EmailAddress);
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get organization");
				return null;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.OrganizationExists",
					new
					{
						organizationId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to validate existence of organization.");
				return false;
			}
		}

		public bool IsUsed(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.OrganizationIsUsed",
					new
					{
						OrganizationId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to determine if organization in use.");
				return false;
			}
		}


		public (long TotalCount, List<Organization> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT id FROM val.ORGANIZATION O";

			if (orClauses.Count > 0)
			{
				//First OR clause gets special treatment - it starts with the WHERE, not an OR
				List<string> orStrings = new List<string>();

				foreach (OrClause orClause in orClauses)
				{
					List<string> andStrings = new List<string>();
					foreach (AndClause andClause in orClause.AndClauses)
					{
						switch (andClause.Property)
						{
							case "name":
								andStrings.Add($"name {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "website":
								andStrings.Add($"organization_url {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "phoneNumber":
								andStrings.Add($"(voice_number {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)} OR fax_number {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							case "email":
								andStrings.Add($"EXISTS (SELECT NULL FROM val.ORGANIZATION_EMAIL E WHERE E.organization_id = O.id AND E.email_address {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							default: break;
						}
					}

					//Combine all of the AND clauses with ANDs, and add as an OrClause
					orStrings.Add(string.Join(" AND ", andStrings));
				}

				//Combine all of the OR clauses with ORs and parens, add it to the query. Each OR clause needs to be wrapped in parens, so the join puts the closing and opening on ones in the middle, but need to add the opening for the first and closing for the last
				query += $" WHERE ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY O.id";

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var organizations = new List<Organization>();

				//Get all the org IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.id).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					//Query for the rest of the data, passing in the IDs for the current page
					var data = db.QueryMultipleFromProcedure("val.OrganizationFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the organizations
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawOrganizations = resultSet.Select(x => (x.Id, x.Name, x.Website, x.VoiceNumber, x.FaxNumber)).ToList();

					//Move to the second result set, the addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawAddresses = resultSet.Select(x => (x.Id, x.OrganizationId, x.Street1, x.Street2, x.Street3, x.Locality, x.Region, x.Country, x.PostalCode)).ToList();

					//Move to the third result set, the email addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawEmailAddresses = resultSet.Select(x => (x.OrganizationId, x.EmailAddress, x.OrderIndex)).ToList();

					//Build the list of organization objects
					foreach (var rawOrg in rawOrganizations.OrderBy(x => x.Id))
					{
						var org = new Organization
						{
							ID = rawOrg.Id,
							Name = rawOrg.Name,
							Website = rawOrg.Website,
							Emails = rawEmailAddresses.Where(x => x.OrganizationId == rawOrg.Id).OrderBy(x => x.OrderIndex).Select(x => (string)x.EmailAddress).ToList()
						};

						//Add the phone numbers collection, which takes some manipulation
						var phoneNumbers = new List<PhoneNumber>();
						if (!string.IsNullOrEmpty(rawOrg.VoiceNumber))
						{
							phoneNumbers.Add(new PhoneNumber { Number = rawOrg.VoiceNumber, Type = "voice" });
						}

						if (!string.IsNullOrEmpty(rawOrg.FaxNumber))
						{
							phoneNumbers.Add(new PhoneNumber { Number = rawOrg.FaxNumber, Type = "fax" });
						}

						if (phoneNumbers.Count > 0)
						{
							org.PhoneNumbers = phoneNumbers;
						}

						//Add the addresses
						org.Addresses = rawAddresses.Where(x => x.OrganizationId == org.ID).Select(x => new Address
						{
							ID = x.Id,
							OrganizationID = x.OrganizationId,
							Street1 = x.Street1,
							Street2 = x.Street2,
							Street3 = x.Street3,
							Locality = x.Locality,
							Region = x.Region,
							Country = x.Country,
							PostalCode = x.PostalCode
						}).ToList();

						//Finally add it to the collection
						organizations.Add(org);
					}
				}

				return (totalRecords, organizations);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get organization list");
				throw;
			}
		}

		public (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationID, List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = $"SELECT id FROM val.PERSON P WHERE org_id = {organizationID}";

			if (orClauses.Count > 0)
			{
				//First OR clause gets special treatment - it starts with the WHERE, not an OR
				List<string> orStrings = new List<string>();

				foreach (OrClause orClause in orClauses)
				{
					List<string> andStrings = new List<string>();
					foreach (AndClause andClause in orClause.AndClauses)
					{
						switch (andClause.Property)
						{
							case "fullName":
								andStrings.Add($"full_name {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "phoneNumber":
								andStrings.Add($"EXISTS (SELECT NULL FROM val.PERSON_PHONE PP WHERE PP.person_id = P.id AND PP.phone_number {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							case "email":
								andStrings.Add($"EXISTS (SELECT NULL FROM val.PERSON_EMAIL E WHERE E.person_id = P.id AND E.email_address {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							default: break;
						}
					}

					//Combine all of the AND clauses with ANDs, and add as an OrClause
					orStrings.Add(string.Join(" AND ", andStrings));
				}

				//Combine all of the OR clauses with ORs and parens, add it to the query. Each OR clause needs to be wrapped in parens, so the join puts the closing and opening on ones in the middle, but need to add the opening for the first and closing for the last
				query += $" AND ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY P.id";

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var contacts = new List<Person>();

				//Get all the IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.id).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					var data = db.QueryMultipleFromProcedure("val.OrganizationContactsFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the Contacts
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawPersons = resultSet.Select(x => (x.PersonId, x.FullName)).ToList();

					//Move to the second result set, the emails
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawEmailAddresses = resultSet.Select(x => (x.PersonId, x.OrderIndex, x.EmailAddress)).ToList();

					//Move to the third result set, the phone numbers
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawPhone = resultSet.Select(x => (x.PersonId, x.OrderIndex, x.PhoneNumber, x.PhoneNumberType)).ToList();

					//Build the list of organization objects
					foreach (var rawPerson in rawPersons.OrderBy(x => x.PersonId))
					{
<<<<<<< Web.Public/src/Providers/OrganizationProvider.cs
						var emails = rawEmailAddresses
							.Where(x => x.PersonId == rawPerson.PersonId)
							.OrderBy(x => x.OrderIndex)
							.Select(x => (string)x.EmailAddress)
							.ToList();

						var phoneNumbers = rawPhone
							.Where(x => x.PersonId == rawPerson.PersonId)
							.OrderBy(x => x.OrderIndex)
							.Select(x => new PhoneNumber()
							{
								Number = x.PhoneNumber,
								Type = x.PhoneNumberType
							})
							.ToList();

						var person = new Person
						{
							ID = rawPerson.PersonId,
							Name = rawPerson.FullName,
							OrganizationID = organizationId,
							Emails = emails.Count > 0 ? emails : null,
							PhoneNumbers = phoneNumbers.Count > 0 ? phoneNumbers : null
						};

						//Finally add it to the collection
						contacts.Add(person);
					}
=======
						ID = rawPerson.PersonId,
						Name = rawPerson.FullName,
						OrganizationID = organizationId,
						Emails = emails.Count > 0 ? emails : null,
						PhoneNumbers = phoneNumbers.Count > 0 ? phoneNumbers : null
					};

					//Finally add it to the collection
					contacts.Add(person);
>>>>>>> Web.Public/src/Providers/OrganizationProvider.cs
				}

				return (totalRecords, contacts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get person list");
				throw;
			}
		}
	}
}