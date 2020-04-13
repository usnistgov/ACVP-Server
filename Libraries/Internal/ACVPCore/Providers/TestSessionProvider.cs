using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

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

		public Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID)
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
					Publishable = publishable,
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

		public Result UpdateStatusFieldsForJava(long testSessionID, DateTime? passedDate, bool? disposition, bool? publishable, bool? published)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.TestSessionStatusUpdateForJava", inParams: new
				{
					TestSessionId = testSessionID,
					PassedDate = passedDate,
					Disposition = disposition,
					Publishable = publishable,
					Published = published
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
					Publishable = testSessionData.publishable,
					Published = testSessionData.published,
					PassedOn = testSessionData.passed_date,
					IsSample = testSessionData.sample,
					UserID = testSessionData.UserId,
					UserName = testSessionData.UserName,
					VectorSets = new List<VectorSet>()
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
						GeneratorVersion = vectorSet.generator_version
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
