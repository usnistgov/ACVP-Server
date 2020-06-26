using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using Serilog;
using Web.Public.Models;
using Web.Public.Helpers;

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
				var data = db.SingleFromProcedure("val.PersonGet", new
				{
					PersonID = id
				});

				if (data == null)
				{
					return null;
				}
				
				var result = new Person
				{
					ID = id,
					Name = data.Name,
					OrganizationID = data.OrganizationID
				};

				var emailData = db.QueryFromProcedure("val.PersonEmailsGet", new
				{
					PersonID = id
				});

				if (emailData != null)
				{
					result.Emails = new List<string>();
					
					foreach (var email in emailData)
					{
						result.Emails.Add(email.EmailAddress);
					}
				}

				var phoneData = db.QueryFromProcedure("val.PersonPhonesGet", new
				{
					PersonID = id
				});

				if (phoneData != null)
				{
					var phoneNumbers = new List<PhoneNumber>();

					foreach (var phone in phoneData)
					{
						phoneNumbers.Add(new PhoneNumber { Number = phone.PhoneNumber, Type = phone.PhoneType });
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
				var data = db.ExecuteProcedure("val.PersonExists",
					new
					{
						personId = id
					},
					new
					{
						exists = false
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
			string query = "SELECT id FROM val.Person P";

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
							case "vendorId":
								andStrings.Add($"org_id {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, long.Parse(andClause.Value))}");
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
				query += $" WHERE ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY P.id";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var people = new List<Person>();

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

					var data = db.QueryMultipleFromProcedure("val.PersonFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the people
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawPeople = resultSet.Select(x => (x.Id, x.OrganizationId, x.Name)).ToList();

					//Move to the second result set, the email addresses
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawEmailAddresses = resultSet.Select(x => (x.PersonId, x.EmailAddress, x.OrderIndex)).ToList();

					//Move to the third result set, the phone numbers
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawPhoneNumbers = resultSet.Select(x => (x.PersonId, x.PhoneNumber, x.PhoneNumberType, x.OrderIndex)).ToList();

					//Build the list of Person objects
					foreach (var rawPerson in rawPeople.OrderBy(x => x.Id))
					{
						var emails = rawEmailAddresses.Where(x => x.PersonId == rawPerson.Id)?.OrderBy(x => x.OrderIndex)?.Select(x => (string)x.EmailAddress)?.ToList();
						var phoneNumbers = rawPhoneNumbers.Where(x => x.PersonId == rawPerson.Id)?.OrderBy(x => x.OrderIndex)?.Select(x => new PhoneNumber { Number = x.PhoneNumber, Type = x.PhoneNumberType })?.ToList();

						people.Add(new Person
						{
							ID = rawPerson.Id,
							Name = rawPerson.Name,
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