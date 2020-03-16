using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class VectorSetService : IVectorSetService
	{
		private readonly IVectorSetProvider _vectorSetProvider;
		private readonly ITestSessionService _testSessionService;

		public VectorSetService(IVectorSetProvider vectorSetProvider, ITestSessionService testSessionService)
		{
			_vectorSetProvider = vectorSetProvider;
			_testSessionService = testSessionService;
		}

		public Result Cancel(long vectorSetID)
		{
			//Cancel the vector set
			Result result = _vectorSetProvider.Cancel(vectorSetID);

			if (result.IsSuccess)
			{
				//Potentially update the Test Session status
				result = _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(vectorSetID);
			}

			return result;
		}

		public Result Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities)
		{
			Result result;

			//Insert the vector set record
			result = _vectorSetProvider.Insert(vectorSetID, testSessionID, generatorVersion, algorithmID);

			if (result.IsSuccess)
			{
				//Insert the capabilities json
				result = _vectorSetProvider.InsertVectorSetJson(vectorSetID, VectorSetJsonFileTypes.Capabilities, capabilities);
			}

			return result;
		}

		public Result InsertSubmittedAnswers(long vectorSetID, string results)
		{
			return _vectorSetProvider.InsertVectorSetJson(vectorSetID, VectorSetJsonFileTypes.SubmittedAnswers, results);
		}

		public Result UpdateStatus(long vectorSetID, VectorSetStatus status) => _vectorSetProvider.UpdateStatus(vectorSetID, status, null);

		public Result RecordError(long vectorSetID, string errorMessage)
		{
			return _vectorSetProvider.UpdateStatus(vectorSetID, VectorSetStatus.Error, errorMessage);
		}

		public List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetsForTestSession(long testSessionID) => _vectorSetProvider.GetVectorSetIDsForTestSession(testSessionID);

		public VectorSet GetVectorSet(long vectorSetId)
		{
			var result = _vectorSetProvider.GetVectorSet(vectorSetId);
			
			if (result == null)
				return null;
			
			result.JsonFilesAvailable = _vectorSetProvider.GetVectorSetJsonFilesAvailable(vectorSetId);
			
			return result;
		}

		public string GetVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType)
		{
			return _vectorSetProvider.GetVectorFileJson(vectorSetId, fileType);
		}
	}
}
