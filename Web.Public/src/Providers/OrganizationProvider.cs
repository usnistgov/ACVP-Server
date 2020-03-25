using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using Serilog;
using Web.Public.Helpers;

namespace Web.Public.Providers
{
    public class OrganizationProvider : IOrganizationProvider
    {
        private readonly string _acvpConnectionString;

		public OrganizationProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public List<Organization> GetList(PagingOptions pagingOptions)
		{
			// var db = new MightyOrm<OrganizationLite>(_acvpConnectionString);
			//
			// try
			// {
			// 	var dbResult = db.QueryWithExpando("val.OrganizationsGet", inParams: new
			// 	{
			// 		param.PageSize,
			// 		PageNumber = param.Page,
			// 		param.Id,
			// 		param.Name
			// 	}, new
			// 	{
			// 		totalRecords = (long)0
			// 	});
			//
			// 	var result = dbResult.Data;
			// 	long totalRecords = dbResult.ResultsExpando.totalRecords;
			// 	
			// 	return result.WrapPagedEnumerable(param.PageSize, param.Page, totalRecords);
			// }
			// catch (Exception ex)
			// {
			// 	Log.Error("Unable to get organization list", ex);
			// 	throw;
			// }
			
			// Needs to be rewritten to use public paging technique of (amount, offset)
			return null;
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
					ID = orgData.id,
					Name = orgData.name,
					Url = orgData.organization_url,
					VoiceNumber = orgData.voice_number,
					FaxNumber = orgData.fax_number,
					Parent = orgData.parent_organization_id == null ? null : new OrganizationLite { ID = orgData.parent_organization_id}
				};

				var addressData = db.QueryFromProcedure("val.OrganizationGetAddresses", inParams: new
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
							ID = address.id,
							OrganizationID = organizationID,
							Street1 = address.address_street1,
							Street2 = address.address_street2,
							Street3 = address.address_street3,
							Locality = address.address_locality,
							Region = address.address_region,
							Country = address.address_country,
							PostalCode = address.address_postal_code
						});
					}
				}

				var personData = db.QueryFromProcedure("val.OrganizationGetPersons", inParams: new
				{
					OrganizationID = organizationID
				});

				if (personData != null)
				{
					result.Persons = new List<PersonLite>();
					
					foreach (var person in personData)
					{
						result.Persons.Add(new PersonLite
						{
							ID = person.id,
							Name = person.full_name
						});
					}
				}

				var emailData = db.QueryFromProcedure("val.OrganizationGetEmails", inParams: new
				{
					OrganizationID = organizationID
				});

				if (emailData != null)
				{
					result.Emails = new List<string>();
					
					foreach (var email in emailData)
					{
						result.Emails.Add(email.email_address);
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				Log.Error("Unable to get organization", ex);
				throw;
			}
		}
    }
}