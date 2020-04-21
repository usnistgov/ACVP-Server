using System;
using NIST.CVP.Libraries.Shared.Results;

using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

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

		public Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.PrerequisitesForScenarioAlgorithmDelete", inParams: new
				{
					ScenarioAlgorithmId = scenarioAlgorithmID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(long scenarioAlgorithmID, long validationID, string requirement)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ScalarFromProcedure("val.PrerequisiteInsert", inParams: new
				{
					ScenarioAlgorithmId = scenarioAlgorithmID,
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
