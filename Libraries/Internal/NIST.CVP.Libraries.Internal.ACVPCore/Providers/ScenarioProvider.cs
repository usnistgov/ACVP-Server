using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;

using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class ScenarioProvider : IScenarioProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public ScenarioProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public InsertResult Insert(long validationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ScenarioInsert", inParams: new
				{
					ValidationId = validationID
				});

				return new InsertResult(data.ScenarioId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public long GetScenarioIDForValidationOE(long validationID, long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ScenariosForValidationOEGet", inParams: new
				{
					ValidationId = validationID, 
					OEId = oeID
				});

				return data == null ? 0 : data.ScenarioId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, null);
				return 0;
			}
		}
	}
}
