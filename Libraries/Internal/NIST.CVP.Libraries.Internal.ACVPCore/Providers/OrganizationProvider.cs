using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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
				db.ExecuteProcedure("val.OrganizationDelete", inParams: new { OrganizationID = organizationID });
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
				db.ExecuteProcedure("val.OrganizationEmailDeleteAll", inParams: new { OrganizationID = organizationID });
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

		public PagedEnumerable<OrganizationLite> Get(OrganizationListParameters param)
		{
			var result = new List<OrganizationLite>();
			long totalRecords = 0;
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

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
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

			Organization result = new Organization
			{
				Parent = new OrganizationLite(),
				Addresses = new List<Address>(),
				Persons = new List<PersonLite>(),
				Emails = new List<string>(),
				ID = organizationID
			};

			try
			{
				var orgData = db.SingleFromProcedure("val.OrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				result.Name = orgData.Name;
				result.Url = orgData.Website;
				result.VoiceNumber = orgData.VoiceNumber;
				result.FaxNumber = orgData.FaxNumber;
				result.Parent = new OrganizationLite
				{
					ID = orgData.ParentOrganizationId ?? 0
				};

				var addressData = db.QueryFromProcedure("val.AddressesForOrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var address in addressData)
				{
					result.Addresses.Add(new Address
					{
						ID = address.ID,
						Street1 = address.Street1,
						Street2 = address.Street2,
						Street3 = address.Street3,
						Locality = address.Locality,
						Region = address.Region,
						Country = address.Country,
						PostalCode = address.PostalCode
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
						ID = person.Id,
						Name = person.FullName
					});
				}

				var emailData = db.QueryFromProcedure("val.OrganizationEmailsGet", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var email in emailData)
				{
					result.Emails.Add(email.EmailAddress);
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
