using System;
using System.Collections.Generic;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class VectorSetProvider : IVectorSetProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<VectorSetProvider> _logger;

		public VectorSetProvider(IConnectionStringFactory connectionStringFactory, ILogger<VectorSetProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Cancel(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.VectorSetCancel @0", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.VectorSetInsert @0, @1, @2, @3", vectorSetID, testSessionID, generatorVersion, algorithmID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result UpdateSubmittedResults(long vectorSetID, string results)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.VectorSetUpdateSubmittedResults @0, @1", vectorSetID, System.Text.Encoding.UTF8.GetBytes(results));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result UpdateStatus(long vectorSetID, VectorSetStatus status, string errorMessage)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.VectorSetUpdateStatusAndMessage @0, @1, @2", vectorSetID, status, errorMessage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetIDsForTestSession(long testSessionID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> vectorSetIDs = new List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)>();

			try
			{
				var data = db.QueryFromProcedure("acvp.VectorSetsForTestSessionGet", inParams: new
				{
					TestSessionId = testSessionID
				});

				foreach (var vectorSet in data)
				{
					vectorSetIDs.Add((vectorSet.VectorSetId, vectorSet.AlgorithmId, (VectorSetStatus)vectorSet.VectorSetStatusId, vectorSet.ErrorMessage ));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return vectorSetIDs;
		}
	}
}
