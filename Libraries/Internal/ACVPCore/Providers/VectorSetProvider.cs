using System;
using System.Collections.Generic;
using ACVPCore.Models;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

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
				db.ExecuteProcedure("acvp.VectorSetCancel", inParams: new
				{
					id = id
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
				db.ExecuteProcedure("acvp.VectorSetInsert", inParams: new
				{
					VectorSetID = vectorSetID,
					TestSessionID = testSessionID,
					GeneratorVersion = generatorVersion,
					AlgorithmID = algorithmID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result UpdateSubmittedResults(long vectorSetID, string results)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.VectorSetUpdateSubmittedResult", inParams: new { VectorSetID = vectorSetID, Results = System.Text.Encoding.UTF8.GetBytes(results) });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result UpdateStatus(long vectorSetID, VectorSetStatus status, string errorMessage)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.VectorSetUpdateStatusAndMessage", inParams: new
				{
					VectorSetID = vectorSetID,
					Status = status,
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
				var data = db.QueryFromProcedure("acvp.VectorSetsForTestSessionGet", inParams: new
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
				var queryResult = db.SingleFromProcedure(
					"acvp.VectorSetGet",
					new
					{
						vectorSetId
					});

				if (queryResult == null)
					return null;

				VectorSet result = new VectorSet()
				{
					Algorithm = queryResult.algorithmName,
					AlgorithmId = queryResult.algorithmId,
					GeneratorVersion = queryResult.generatorVersion,
					Id = queryResult.vectorSetId,
					Status = (VectorSetStatus)queryResult.status
				};

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public List<VectorSetJsonFileTypes> GetVectorSetJsonFilesAvailable(long vectorSetId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var queryResult = db.QueryFromProcedure(
					"acvp.VectorSetGetJsonFIleTypes",
					new
					{
						vectorSetId
					});

				if (queryResult == null)
					return new List<VectorSetJsonFileTypes>();

				List<VectorSetJsonFileTypes> result = new List<VectorSetJsonFileTypes>();
				foreach (var item in queryResult)
				{
					result.Add(Enum.Parse<VectorSetJsonFileTypes>(item.fileType, true));
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
				var queryResult = db.SingleFromProcedure(
					"acvp.VectorSetJsonGet",
					new
					{
						VsId = vectorSetId,
						JsonFileType = fileType.ToString()
					});

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
				var data = db.QueryFromProcedure("acvp.VectorSetJsonGetAll", inParams: new
				{
					VectorSetId = vectorSetID
				});

				foreach (var row in data)
				{
					vectorSetJson.Add(((VectorSetJsonFileTypes)row.FileType, row.Content, row.CreatedOn));
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
				db.ExecuteProcedure("acvp.VectorSetJsonPut", inParams: new
				{
					VsId = vectorSetID,
					JsonFileType = fileType.ToString(),
					Content = json
				});

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
				db.ExecuteProcedure("acvp.VectorSetArchive", inParams: new
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
				var data = db.QueryFromProcedure("acvp.VectorSetsToArchiveGet");

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
				db.ExecuteProcedure("acvp.VectorSetRemoveJson",
					new
					{
						vectorSetId,
						fileTypeId = fileType
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
