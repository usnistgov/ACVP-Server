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
				var orgData = db.SingleFromProcedure("dbo.OrganizationGet", inParams: new
				{
					OrganizationId = organizationID
				});

				if (orgData == null)
					return null;

				var result = new Organization
				{
					ID = organizationID,
					Name = orgData.OrganizationName,
					Website = orgData.OrganizationUrl,
					ParentOrganizationID = orgData.ParentOrganizationId
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

				var emailData = db.QueryFromProcedure("dbo.OrganizationEmailsGet", inParams: new
				{
					OrganizationId = organizationID
				});

				if (emailData?.Count() > 0 )
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
				var data = db.ExecuteProcedure("dbo.OrganizationExists", inParams:
					new
					{
						OrganizationId = id
					}, outParams:
					new
					{
						Exists = false
					});

				return data.Exists;
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
				var data = db.ExecuteProcedure("dbo.OrganizationIsUsed", inParams:
					new
					{
						OrganizationId = id
					}, outParams:
					new
					{
						IsUsed = false
					});

				return data.IsUsed;
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
			string query = "SELECT OrganizationId FROM dbo.Organizations O";

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
								andStrings.Add($"OrganizationName {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "website":
								andStrings.Add($"OrganizationUrl {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "phoneNumber":
								andStrings.Add($"(VoiceNumber {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)} OR FaxNumber {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							case "email":
								andStrings.Add($"EXISTS (SELECT NULL FROM dbo.OrganizationEmails E WHERE E.OrganizationId = O.OrganizationId AND E.EmailAddress {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
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

			query += " ORDER BY O.OrganizationId";

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var organizations = new List<Organization>();

				//Get all the org IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.OrganizationId).ToArray();

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
					var data = db.QueryMultipleFromProcedure("dbo.OrganizationFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the organizations
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawOrganizations = resultSet.Select(x => (x.OrganizationId, x.OrganizationName, x.OrganizationUrl, x.VoiceNumber, x.FaxNumber, x.ParentOrganizationId)).ToList();

					//Move to the second result set, the addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawAddresses = resultSet.Select(x => (x.AddressId, x.OrganizationId, x.Street1, x.Street2, x.Street3, x.Locality, x.Region, x.Country, x.PostalCode)).ToList();

					//Move to the third result set, the email addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawEmailAddresses = resultSet.Select(x => (x.OrganizationId, x.EmailAddress, x.OrderIndex)).ToList();

					//Build the list of organization objects
					foreach (var rawOrg in rawOrganizations.OrderBy(x => x.OrganizationId))
					{
						var org = new Organization
						{
							ID = rawOrg.OrganizationId,
							Name = rawOrg.OrganizationName,
							Website = rawOrg.OrganizationUrl,
							ParentOrganizationID = rawOrg.ParentOrganizationId
						};

						//Add the email addresses, if they have any
						List<string> emails = rawEmailAddresses.Where(x => x.OrganizationId == rawOrg.OrganizationId).OrderBy(x => x.OrderIndex).Select(x => (string)x.EmailAddress).ToList();
						if (emails.Count > 0)
						{
							org.Emails = emails;
						}

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

						//Add the addresses, if they have any
						List<Address> addresses = rawAddresses.Where(x => x.OrganizationId == org.ID).Select(x => new Address
						{
							ID = x.AddressId,
							OrganizationID = x.OrganizationId,
							Street1 = x.Street1,
							Street2 = x.Street2,
							Street3 = x.Street3,
							Locality = x.Locality,
							Region = x.Region,
							Country = x.Country,
							PostalCode = x.PostalCode
						}).ToList();

						if (addresses.Count > 0)
						{
							org.Addresses = addresses;
						}

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
			string query = $"SELECT PersonId FROM dbo.People P WHERE OrganizationId = {organizationID}";

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
								andStrings.Add($"FullName {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "phoneNumber":
								andStrings.Add($"EXISTS (SELECT NULL FROM dbo.PersonPhoneNumbers PP WHERE PP.PersonId = P.PersonId AND PP.PhoneNumber {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
								break;
							case "email":
								andStrings.Add($"EXISTS (SELECT NULL FROM dbo.PersonEmails E WHERE E.PersonId = P.PersonId AND E.EmailAddress {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)})");
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

			query += " ORDER BY P.PersonId";

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var contacts = new List<Person>();

				//Get all the IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.PersonId).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					var data = db.QueryMultipleFromProcedure("dbo.OrganizationContactsFilteredListDataGet", inParams: new
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
							OrganizationID = organizationID,
							Emails = emails.Count > 0 ? emails : null,
							PhoneNumbers = phoneNumbers.Count > 0 ? phoneNumbers : null
						};

						//Finally add it to the collection
						contacts.Add(person);
					}
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