using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
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
				db.ExecuteProcedure("dbo.OrganizationDelete", inParams: new { OrganizationId = organizationID });
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
				db.ExecuteProcedure("dbo.OrganizationEmailDeleteAll", inParams: new { OrganizationId = organizationID });
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
				var data = db.SingleFromProcedure("dbo.OrganizationInsert", inParams: new
				{
					Name = name,
					Website = website,
					VoiceNumber = voiceNumber,
					FaxNumber = faxNumber,
					ParentOrganizationId = parentOrganizationID
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Organization");
				}
				else
				{
					return new InsertResult((long)data.OrganizationId);
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
				var dbResult = db.QueryWithExpando("dbo.OrganizationsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					OrganizationId = param.Id,
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
				db.ExecuteProcedure("dbo.OrganizationEmailInsert", inParams: new
				{
					OrganizationId = organizationID,
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
				db.ExecuteProcedure("dbo.OrganizationUpdate", inParams: new
				{
					OrganizationId = organizationID,
					Name = name,
					Website = website,
					VoiceNumber = voiceNumber,
					FaxNumber = faxNumber,
					ParentOrganizationId = parentOrganizationID,
					NameUpdated = nameUpdated,
					WebsiteUpdated = websiteUpdated,
					VoiceNumberUpdated = voiceNumberUpdated,
					FaxNumberUpdated = faxNumberUpdated,
					ParentOrganizationIdUpdated = parentOrganizationIDUpdated
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
				var data = db.SingleFromProcedure("dbo.OrganizationIsUsed", inParams: new
				{
					OrganizationId = organizationID
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
				ID = organizationID
			};

			try
			{
				var orgData = db.SingleFromProcedure("dbo.OrganizationGet", inParams: new
				{
					OrganizationId = organizationID
				});

				result.Name = orgData.OrganizationName;
				result.Url = orgData.OrganizationUrl;
				result.VoiceNumber = orgData.VoiceNumber;
				result.FaxNumber = orgData.FaxNumber;
				result.Parent = new OrganizationLite
				{
					ID = orgData.ParentOrganizationId ?? 0
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result;
		}

		public bool OrganizationExists(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.OrganizationExists", inParams: new
				{
					OrganizationId = organizationID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}

		public List<string> GetEmails(long organizationID)
		{
			List<string> result = new List<string>();
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var emailData = db.QueryFromProcedure("dbo.OrganizationEmailsGet", inParams: new
				{
					OrganizationId = organizationID
				});

				foreach (var email in emailData)
				{
					result.Add(email.EmailAddress);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return result;
		}
	}
}
