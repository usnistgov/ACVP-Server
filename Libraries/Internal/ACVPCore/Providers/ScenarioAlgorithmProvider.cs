using System;
using System.Collections.Generic;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Results;

namespace ACVPCore.Providers
{
	public class ScenarioAlgorithmProvider : IScenarioAlgorithmProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public ScenarioAlgorithmProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public InsertResult Insert(long scenarioID, long algorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ScenarioAlgorithmInsert", inParams: new
				{
					ScenarioId = scenarioID,
					AlgorithmId = algorithmID
				});

				return new InsertResult(data.ScenarioAlgorithmId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithmsForScenario(long scenarioID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long ScenarioAlgorithmID, long AlgorithmID)> scenarioAlgorithms = new List<(long ScenarioAlgorithmID, long AlgorithmID)>();
			try
			{
				var data = db.QueryFromProcedure("val.ScenarioAlgorithmsForScenarioGet", inParams: new
				{
					ScenarioId = scenarioID
				});

				foreach (var scenarioAlgorithm in data)
				{
					scenarioAlgorithms.Add((scenarioAlgorithm.ScenarioAlgorithmId, scenarioAlgorithm.AlgorithmId));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return scenarioAlgorithms;
		}

		public Result Delete(long scenarioAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.ScenarioAlgorithmDelete", inParams: new { ScenarioAlgorithmId = scenarioAlgorithmID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}
	}
}
