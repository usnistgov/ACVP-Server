using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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

		public Result CancelVectorSets(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.TestSessionVectorSetsCancel", inParams: new { id = id });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, long userID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.TestSessionInsert", inParams: new
				{
					TestSessionID = testSessionId,
					ACVVersionID = acvVersionID,
					Generator = generator,
					IsSample = isSample,
					UserID = userID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
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
				_logger.LogError(ex);
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
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param)
		{
			var result = new List<TestSessionLite>();
			long totalRecords = 0;
			var db = new MightyOrm<TestSessionLite>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("acvp.TestSessionsGet",
					new
					{
						param.PageSize,
						param.Page,
						param.TestSessionId,
						param.TestSessionStatus,
						param.VectorSetId
					}, new
					{
						totalRecords = (long)0
					});

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public TestSession Get(long testSessionId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var testSessionData = db.SingleFromProcedure(
					"acvp.TestSessionGetById",
					new
					{
						testSessionId
					});

				if (testSessionData == null)
					return null;

				return new TestSession()
				{
					TestSessionId = testSessionId,
					Created = testSessionData.created_on,
					Status = (TestSessionStatus)testSessionData.TestSessionStatusId,
					IsSample = testSessionData.sample,
					UserID = testSessionData.UserId,
					UserName = testSessionData.UserName
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public List<VectorSet> GetVectorSetsForTestSession(long testSessionId)
		{
			var result = new List<VectorSet>();
			var db = new MightyOrm(_acvpConnectionString);

			try
			{

				var vectorSetsData = db.QueryFromProcedure(
					"acvp.VectorSetsGetByTestSessionId",
					new
					{
						testSessionId
					});
				foreach (var vectorSet in vectorSetsData)
				{
					result.Add(new VectorSet()
					{
						Algorithm = vectorSet.display_name,
						Id = vectorSet.id,
						Status = (VectorSetStatus)vectorSet.status,
						AlgorithmId = vectorSet.algorithm_id,
						GeneratorVersion = vectorSet.generator_version,
						TestSessionID = testSessionId
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result;
		}

		public bool TestSessionExists(long testSessionID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("acvp.TestSessionExists", inParams: new
				{
					TestSessionId = testSessionID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}

		public long GetTestSessionIDFromVectorSet(long vectorSetID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (long)db.ScalarFromProcedure("acvp.TestSessionIdGet", inParams: new
				{
					VectorSetId = vectorSetID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return -1;
			}
		}

		public void Expire(int ageInDays)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.TestSessionsExpire", inParams: new
				{
					AgeInDays = ageInDays
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
		}
	}
}
