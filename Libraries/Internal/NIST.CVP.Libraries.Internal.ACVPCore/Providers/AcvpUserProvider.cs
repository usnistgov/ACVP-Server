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
	public class AcvpUserProvider : IAcvpUserProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<AcvpUserProvider> _logger;

		public AcvpUserProvider(IConnectionStringFactory connectionStringFactory, ILogger<AcvpUserProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public PagedEnumerable<AcvpUserLite> GetList(AcvpUserListParameters param)
		{
			List<AcvpUserLite> result = new List<AcvpUserLite>();
			long totalRecords = 0;
			var db = new MightyOrm<AcvpUserLite>(_acvpConnectionString);

			try
			{
				var dbData = db.QueryWithExpando("dbo.ACVPUsersGet",
					inParams: new
					{
						PageSize = param.PageSize,
						PageNumber = param.Page,
						ACVPUserId = param.AcvpUserId,
						PersonId = param.PersonId,
						OrganizationName = param.CompanyName,
						PersonName = param.PersonName
					},
					new
					{
						totalRecords = (long)0
					});

				result = dbData.Data;

				totalRecords = dbData.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}

			return result.ToPagedEnumerable(param.PageSize, param.PageSize, totalRecords);
		}

		public AcvpUser Get(long userId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.ACVPUserGetById", new
				{
					ACVPUserId = userId
				});

				if (data == null)
					return null;

				return new AcvpUser()
				{
					OrgnizationID = data.OrganizationId,
					OrganizationName = data.OrganizationName,
					FullName = data.FullName,
					PersonID = data.PersonId,
					ACVPUserID = data.ACVPUserId,
					CertificateBase64 = Convert.ToBase64String((byte[])data.Certificate),
					Seed = data.Seed,
					CommonName = data.CommonName
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return null;
			}
		}

		public Result UpdateSeed(long userId, string seed)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.AcvpUserSetSeed", new
				{
					ACVPUserId = userId,
					Seed = seed
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return new Result("Failed updating seed.");
			}

			return new Result();
		}

		public Result UpdateCertificate(long userId, string subject, byte[] rawData, DateTime expiresOn)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.ACVPUserSetCertificate", new
				{
					ACVPUserId = userId,
					CommonName = subject,
					Certificate = rawData,
					ExpiresOn = expiresOn
				});
				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return new Result("Failed updating certificate.");
			}

		}

		public Result Insert(long personID, string subject, byte[] rawData, string seed, DateTime expiresOn)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var acvpUserQueryData = db.SingleFromProcedure("dbo.ACVPUserInsert", new
				{
					PersonID = personID,
					CommonName = subject,
					Certificate = rawData,
					Seed = seed,
					ExpiresOn = expiresOn
				});

				return new InsertResult((long)acvpUserQueryData.ACVPUserId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new InsertResult("Unspecified error in ACVP User creation");
			}
		}

		public Result Delete(long userId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("dbo.ACVPUserDelete", new
				{
					ACVPUserId = userId
				});

				return new Result();
			}
			catch (Exception ex)
			{
				return new DeleteResult(ex.Message);
			}
		}
	}
}