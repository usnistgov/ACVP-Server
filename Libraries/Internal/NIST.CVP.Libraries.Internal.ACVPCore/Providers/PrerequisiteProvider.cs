using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class PrerequisiteProvider : IPrerequisiteProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public PrerequisiteProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PrerequisitesForValidationOEAlgorithmDelete", inParams: new
				{
					ValidationOEAlgorithmId = validationOEAlgorithmID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(long validationOEAlgorithmID, long validationID, string requirement)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ScalarFromProcedure("dbo.PrerequisiteInsert", inParams: new
				{
					ValidationOEAlgorithmId = validationOEAlgorithmID,
					ValidationId = validationID,
					Requirement = requirement
				});

				return new InsertResult((long)data);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}
	}
}
