using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class TestSessionProvider : ITestSessionProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public TestSessionProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Cancel(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.TestSessionCancel @0", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result CancelVectorSets(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.TestSessionVectorSetsCancel @0", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.TestSessionInsert @0, @1, @2, @3, @4, @5", testSessionId, acvVersionID, generator, isSample, publishable, userID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public TestSessionStatus GetStatus(long testSessionID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("acvp.TestSessionStatusGet", inParams: new
				{
					TestSessionId = testSessionID
				});

				return (TestSessionStatus)data.TestSessionStatusId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return TestSessionStatus.Unknown;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.TestSessionStatusUpdate", inParams: new
				{
					TestSessionId = testSessionID,
					TestSessionStatusId = testSessionStatus
				});
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
