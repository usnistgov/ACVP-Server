using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using Serilog;
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
					result.PhoneNumbers = new List<PhoneNumber>();
					
					foreach (var phone in phoneData)
					{
						result.PhoneNumbers.Add(new PhoneNumber { Number = phone.PhoneNumber, Type = phone.PhoneType });
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
		
		public (long TotalCount, List<Person> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.QueryMultipleFromProcedure("val.PersonFilteredListGet", inParams: new
				{
					Filter = filter,
					Limit = limit,
					Offset = offset,
					ORdelimiter = orDelimiter,
					ANDdelimiter = andDelimiter
				});

				//Create the objects to hold the final data
				long totalRecords;
				var people = new List<Person>();

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the total records
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				totalRecords = resultSet.First().TotalRecords;

				//Move to the second result set, the people
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawPeople = resultSet.Select(x => (x.Id, x.OrganizationId, x.Name)).ToList();

				//Move to the third result set, the email addresses
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawEmailAddresses = resultSet.Select(x => (x.PersonId, x.EmailAddress, x.OrderIndex)).ToList();

				//Move to the fourth result set, the phone numbers
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