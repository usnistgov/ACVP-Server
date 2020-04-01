using System;
using System.Collections.Generic;
using System.Linq;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
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
				_logger.LogError("Unable to get person list", ex);
				throw;
			}
		}
	}
}