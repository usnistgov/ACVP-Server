using System;
using System.Collections.Generic;
using System.Linq;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
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
				_logger.LogError("Unable to get organization", ex);
				throw;
			}
		}

		public (long TotalCount, List<Organization> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryMultipleFromProcedure("val.OrganizationFilteredListGet", inParams: new
				{
					Filter = filter,
					Limit = limit,
					Offset = offset,
					ORdelimiter = orDelimiter,
					ANDdelimiter = andDelimiter
				});

				//Create the objects to hold the final data
				long totalRecords;
				var organizations = new List<Organization>();

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the total records
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				totalRecords = resultSet.First().TotalRecords;

				//Move to the second result set, the organizations
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawOrganizations = resultSet.Select(x => (x.Id, x.Name, x.Website, x.VoiceNumber, x.FaxNumber)).ToList();

				//Move to the third result set, the addresses
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawAddresses = resultSet.Select(x => (x.Id, x.OrganizationId, x.Street1, x.Street2, x.Street3, x.Locality, x.Region, x.Country, x.PostalCode)).ToList();

				//Move to the fourth result set, the email addresses
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

				return (totalRecords, organizations);
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to get organization list", ex);
				throw;
			}
		}
	}
}