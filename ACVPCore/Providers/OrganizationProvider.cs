using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class OrganizationProvider : IOrganizationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<OrganizationProvider> _logger;

		public OrganizationProvider(IConnectionStringFactory connectionStringFactory, ILogger<OrganizationProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Delete(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.OrganizationDelete @0", organizationID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllEmails(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.OrganizationEmailDeleteAll @0", organizationID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string name, string website, string voiceNumber, string faxNumber, long? parentOrganizationID)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.OrganizationInsert", inParams: new
				{
					Name = name,
					Website = website,
					VoiceNumber = voiceNumber,
					FaxNumber = faxNumber,
					ParentOrganizationID = parentOrganizationID
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Organization");
				}
				else
				{
					return new InsertResult((long)data.OrganizationID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result InsertEmailAddress(long organizationID, string emailAddress, int orderIndex)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//There is no ID on the record, so don't return anything
				db.ExecuteProcedure("val.OrganizationEmailInsert", inParams: new
				{
					OrganizationID = organizationID,
					EmailAddress = emailAddress,
					OrderIndex = orderIndex
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long organizationID, string name, string website, string voiceNumber, string faxNumber, long? parentOrganizationID, bool nameUpdated, bool websiteUpdated, bool voiceNumberUpdated, bool faxNumberUpdated, bool parentOrganizationIDUpdated)
		{
			if (nameUpdated && string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.OrganizationUpdate", inParams: new
				{
					OrganizationID = organizationID,
					Name = name,
					Website = website,
					VoiceNumber = voiceNumber,
					FaxNumber = faxNumber,
					ParentOrganizationID = parentOrganizationID,
					NameUpdated = nameUpdated,
					WebsiteUpdated = websiteUpdated,
					VoiceNumberUpdated = voiceNumberUpdated,
					FaxNumberUpdated = faxNumberUpdated,
					ParentOrganizationIDUpdated = parentOrganizationIDUpdated
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool OrganizationIsUsed(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.OrganizationIsUsed", inParams: new
				{
					OrganizationID = organizationID
				});

				return data.IsUsed;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public Organization Get(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			Organization result = new Organization();
			result.Parent = new OrganizationLite();
			result.Addresses = new List<Address>();
			result.Persons = new List<PersonLite>();
			result.Emails = new List<string>();
			result.ID = organizationID;

			try
			{
				var orgData = db.SingleFromProcedure("val.OrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				result.Name = orgData.name;
				result.Url = orgData.organization_url;
				result.VoiceNumber = orgData.voice_number;
				result.FaxNumber = orgData.fax_number;
				result.Parent = new OrganizationLite();
				if (orgData.parent_organization_id == null) 
				{
					result.Parent.ID = 0; 
				}
				else 
				{ 
					result.Parent.ID = orgData.parent_organization_id; 
				}

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
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return result;
		}
		public bool OrganizationExists(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("val.OrganizationExists", inParams: new
				{
					OrganizationId = oeID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}
	}
}
