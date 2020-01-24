using System;
using System.Collections.Generic;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
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

		public List<long> GetScenarioIDsForValidation(long validationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<long> scenarioIDs = new List<long>();
			try
			{
				var data = db.QueryFromProcedure("val.ScenariosForValidationGet", inParams: new
				{
					ValidationId = validationID
				});

				foreach (var scenario in data)
				{
					scenarioIDs.Add(scenario.ScenarioId);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return scenarioIDs;
		}
	}
}
