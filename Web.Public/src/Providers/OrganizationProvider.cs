using System;
using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class OrganizationProvider : IOrganizationProvider
    {
        private readonly string _acvpConnectionString;

		public OrganizationProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public PagedEnumerable<OrganizationLite> GetList(OrganizationListParameters param)
		{
			var db = new MightyOrm<OrganizationLite>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("val.OrganizationsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					Id = param.Id,
					Name = param.Name
				}, new
				{
					totalRecords = (long)0
				});

				var result = dbResult.Data;
				long totalRecords = dbResult.ResultsExpando.totalRecords;
				
				return result.WrapPagedEnumerable(param.PageSize, param.Page, totalRecords);
			}
			catch (Exception ex)
			{
				Log.Error("Unable to get organization list", ex);
				throw;
			}
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
					Name = orgData.name,
					Url = orgData.organization_url,
					VoiceNumber = orgData.voice_number,
					FaxNumber = orgData.fax_number,
					Parent = new OrganizationLite
					{
						ID = orgData.parent_organization_id == null ? 0 : (long) orgData.parent_organization_id
					}
				};

				var addressData = db.QueryFromProcedure("val.OrganizationGetAddresses", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var address in addressData)
				{
					result.Addresses.Add(new Address
					{
						ID = address.id,
						Street1 = address.address_street1,
						Street2 = address.address_street2,
						Street3 = address.address_street3,
						Locality = address.address_locality,
						Region = address.address_region,
						Country = address.address_country,
						PostalCode = address.address_postal_code
					});
				}

				var personData = db.QueryFromProcedure("val.OrganizationGetPersons", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var person in personData)
				{
					result.Persons.Add(new PersonLite
					{
						ID = person.id,
						Name = person.full_name
					});
				}

				var emailData = db.QueryFromProcedure("val.OrganizationEmailsGet", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var email in emailData)
				{
					result.Emails.Add(email.email_address);
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