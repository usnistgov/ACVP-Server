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
				db.ExecuteProcedure("dbo.TestSessionVectorSetsCancel", inParams: new { TestSessionId = id });
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
				db.ExecuteProcedure("dbo.TestSessionInsert", inParams: new
				{
					TestSessionId = testSessionId,
					ACVVersionId = acvVersionID,
					Generator = generator,
					IsSample = isSample,
					UserId = userID
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
				var data = db.SingleFromProcedure("dbo.TestSessionStatusGet", inParams: new
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
				db.ExecuteProcedure("dbo.TestSessionStatusUpdate", inParams: new
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
				var dbResult = db.QueryWithExpando("dbo.TestSessionsGet", inParams:
					new
					{
						PageSize = param.PageSize,
						Page = param.Page,
						TestSessionId = param.TestSessionId,
						TestSessionStatusId = param.TestSessionStatus,
						VectorSetId = param.VectorSetId
					}, outParams: new
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
				var testSessionData = db.SingleFromProcedure("dbo.TestSessionGetById",
					new
					{
						TestSessionId = testSessionId
					});

				if (testSessionData == null)
					return null;

				return new TestSession()
				{
					TestSessionId = testSessionId,
					Created = testSessionData.CreatedOn,
					Status = (TestSessionStatus)testSessionData.TestSessionStatusId,
					IsSample = testSessionData.IsSample,
					UserID = testSessionData.PersonId,
					UserName = testSessionData.FullName
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
				var vectorSetsData = db.QueryFromProcedure("dbo.VectorSetsGetByTestSessionId",
					new
					{
						TestSessionId = testSessionId
					});

				foreach (var vectorSet in vectorSetsData)
				{
					result.Add(new VectorSet()
					{
						Algorithm = vectorSet.DisplayName,
						Id = vectorSet.VectorSetId,
						Status = (VectorSetStatus)vectorSet.VectorSetStatusId,
						AlgorithmId = vectorSet.AlgorithmId,
						GeneratorVersion = vectorSet.GeneratorVersion,
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
				return (bool)db.ScalarFromProcedure("dbo.TestSessionExists", inParams: new
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
				return (long)db.ScalarFromProcedure("dbo.TestSessionIdGet", inParams: new
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
				db.ExecuteProcedure("dbo.TestSessionsExpire", inParams: new
				{
					AgeInDays = ageInDays
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
		}

		public List<(long TestSessionID, long PersonID)> GetTestSessionsForExpirationWarning(int ageInDaysForWarning)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long TestSessionID, long PersonID)> result = new List<(long TestSessionID, long PersonID)>();

			try
			{
				var data = db.QueryFromProcedure("dbo.TestSessionsForExpirationWarningGet", inParams: new
				{
					AgeInDaysForWarning = ageInDaysForWarning
				});

				foreach (var row in data)
				{
					result.Add((row.TestSessionId, row.PersonId));
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result;
		}

		public void KeepAlive(long testSessionID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TestSessionLastTouchedUpdate", inParams: new
				{
					TestSessionId = testSessionID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
		}
	}
}
