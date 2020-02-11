using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class OrganizationProvider : IOrganizationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<DependencyProvider> _logger;

		public OrganizationProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
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
	}
}
