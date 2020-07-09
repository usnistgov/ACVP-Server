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
	public class PersonProvider : IPersonProvider
	{
		private readonly ILogger<PersonProvider> _logger;
		private readonly string _acvpPublicConnectionString;

		public PersonProvider(IConnectionStringFactory connectionStringFactory, ILogger<PersonProvider> logger)
		{
			_logger = logger;
			_acvpPublicConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public Person Get(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.PersonGet", inParams: new
				{
					PersonId = id
				});

				if (data == null)
				{
					return null;
				}
				
				var result = new Person
				{
					ID = id,
					Name = data.FullName,
					OrganizationID = data.OrganizationId
				};

				var emailData = db.QueryFromProcedure("dbo.PersonEmailsGet", inParams: new
				{
					PersonId = id
				});

				if (emailData != null)
				{
					result.Emails = new List<string>();
					
					foreach (var email in emailData)
					{
						result.Emails.Add(email.EmailAddress);
					}
				}

				var phoneData = db.QueryFromProcedure("dbo.PersonPhonesGet", inParams: new
				{
					PersonId = id
				});

				if (phoneData != null)
				{
					var phoneNumbers = new List<PhoneNumber>();

					foreach (var phone in phoneData)
					{
						phoneNumbers.Add(new PhoneNumber { Number = phone.PhoneNumber, Type = phone.PhoneTypeType });
					}

					if (phoneNumbers.Count > 0)
					{
						result.PhoneNumbers = phoneNumbers;
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Unable to find person with id {id}");
				throw;
			}
		}
		
		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("dbo.PersonExists", inParams:
					new
					{
						PersonId = id
					}, outParams:
					new
					{
						Exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to validate existence of person.");
				return false;
			}
		}
		
		public (long TotalCount, List<Person> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT PersonId FROM dbo.People P";

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
							case "vendorId":
								andStrings.Add($"OrganizationId {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, long.Parse(andClause.Value))}");
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
				query += $" WHERE ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY P.PersonId";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var people = new List<Person>();

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

					var data = db.QueryMultipleFromProcedure("dbo.PersonFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the people
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawPeople = resultSet.Select(x => (x.PersonId, x.OrganizationId, x.FullName)).ToList();

					//Move to the second result set, the email addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawEmailAddresses = resultSet.Select(x => (x.PersonId, x.EmailAddress, x.OrderIndex)).ToList();

					//Move to the third result set, the phone numbers
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawPhoneNumbers = resultSet.Select(x => (x.PersonId, x.PhoneNumber, x.PhoneNumberType, x.OrderIndex)).ToList();

					//Build the list of Person objects
					foreach (var rawPerson in rawPeople.OrderBy(x => x.PersonId))
					{
						var emails = rawEmailAddresses.Where(x => x.PersonId == rawPerson.PersonId)?.OrderBy(x => x.OrderIndex)?.Select(x => (string)x.EmailAddress)?.ToList();
						var phoneNumbers = rawPhoneNumbers.Where(x => x.PersonId == rawPerson.PersonId)?.OrderBy(x => x.OrderIndex)?.Select(x => new PhoneNumber { Number = x.PhoneNumber, Type = x.PhoneNumberType })?.ToList();

						people.Add(new Person
						{
							ID = rawPerson.PersonId,
							Name = rawPerson.FullName,
							OrganizationID = rawPerson.OrganizationId,
							Emails = emails.Count > 0 ? emails : null,
							PhoneNumbers = phoneNumbers.Count > 0 ? phoneNumbers : null
						});
					}
				}

				return (totalRecords, people);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get person list");
				throw;
			}
		}
	}
}