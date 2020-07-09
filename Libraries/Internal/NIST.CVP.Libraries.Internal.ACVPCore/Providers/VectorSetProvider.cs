using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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
				db.ExecuteProcedure("dbo.VectorSetCancel", inParams: new
				{
					VectorSetId = id
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.VectorSetInsert", inParams: new
				{
					VectorSetId = vectorSetID,
					TestSessionId = testSessionID,
					GeneratorVersion = generatorVersion,
					AlgorithmId = algorithmID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		//public Result UpdateSubmittedResults(long vectorSetID, string results)
		//{
		//	var db = new MightyOrm(_acvpConnectionString);

		//	try
		//	{
		//		db.ExecuteProcedure("acvp.VectorSetUpdateSubmittedResult", inParams: new { VectorSetID = vectorSetID, Results = System.Text.Encoding.UTF8.GetBytes(results) });
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex);
		//		return new Result(ex.Message);
		//	}

		//	return new Result();
		//}

		public Result UpdateStatus(long vectorSetID, VectorSetStatus status, string errorMessage)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.VectorSetUpdateStatusAndMessage", inParams: new
				{
					VectorSetId = vectorSetID,
					VectorSetStatusId = status,
					ErrorMessage = errorMessage
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
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
				var data = db.QueryFromProcedure("dbo.VectorSetsForTestSessionGet", inParams: new
				{
					TestSessionId = testSessionID
				});

				foreach (var vectorSet in data)
				{
					vectorSetIDs.Add((vectorSet.VectorSetId, vectorSet.AlgorithmId, (VectorSetStatus)vectorSet.VectorSetStatusId, vectorSet.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return vectorSetIDs;
		}

		public VectorSet GetVectorSet(long vectorSetId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var queryResult = db.SingleFromProcedure("dbo.VectorSetGet",
					new
					{
						VectorSetId = vectorSetId
					});

				if (queryResult == null)
					return null;

				VectorSet result = new VectorSet()
				{
					Algorithm = queryResult.DisplayName,
					AlgorithmId = queryResult.AlgorithmId,
					GeneratorVersion = queryResult.GeneratorVersion,
					Id = queryResult.VectorSetId,
					Status = (VectorSetStatus)queryResult.VectorSetStatusId,
					TestSessionID = queryResult.TestSessionId
				};

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public List<VectorSetJsonFile> GetVectorSetJsonFilesAvailable(long vectorSetId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var queryResult = db.QueryFromProcedure("dbo.VectorSetGetJsonFileTypes",
					new
					{
						vectorSetId
					});

				List<VectorSetJsonFile> result = new List<VectorSetJsonFile>();

				if (queryResult == null)
					return result;
								
				foreach (var item in queryResult)
				{
					result.Add(new VectorSetJsonFile
					{
						Type = (VectorSetJsonFileTypes)item.VectorSetJsonFileTypeId,
						CreatedOn = item.createdOn
					});
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public string GetVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var queryResult = db.SingleFromProcedure("dbo.VectorSetJsonGet",
					new
					{
						VectorSetId = vectorSetId,
						VectorSetJsonFileTypeId = fileType
					}, commandTimeout: 120);

				if (queryResult == null)
					return null;

				return queryResult.Content;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)> GetVectorFileJson(long vectorSetID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)> vectorSetJson = new List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)>();

			try
			{
				var data = db.QueryFromProcedure("dbo.VectorSetJsonGetAll", inParams: new
				{
					VectorSetId = vectorSetID
				}, commandTimeout: 120);

				foreach (var row in data)
				{
					vectorSetJson.Add(((VectorSetJsonFileTypes)row.VectorSetJsonFileTypeId, row.Content, row.CreatedOn));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return vectorSetJson;
		}

		public Result InsertVectorSetJson(long vectorSetID, VectorSetJsonFileTypes fileType, string json)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.VectorSetJsonPut", inParams: new
				{
					VectorSetId = vectorSetID,
					VectorSetJsonFileTypeId = fileType,
					Content = json
				},
				commandTimeout: 120);

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}
		}

		public Result Archive(long vectorSetId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.VectorSetArchive", inParams: new
				{
					VectorSetId = vectorSetId
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public List<long> GetVectorSetsToArchive()
		{
			List<long> vectorSetIDs = new List<long>();

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryFromProcedure("dbo.VectorSetsToArchiveGet");

				foreach (var row in data)
				{
					vectorSetIDs.Add(row.VectorSetId);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return vectorSetIDs;
		}

		public Result RemoveVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.VectorSetRemoveJson",
					new
					{
						VectorSetId = vectorSetId,
						VectorSetJsonFileTypeId = fileType
					});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}
	}
}
