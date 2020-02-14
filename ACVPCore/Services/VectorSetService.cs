using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class VectorSetService : IVectorSetService
	{
		private readonly IVectorSetProvider _vectorSetProvider;
		private readonly IVectorSetExpectedResultsProvider _vectorSetExpectedResultsProvider;

		public VectorSetService(IVectorSetProvider vectorSetProvider, IVectorSetExpectedResultsProvider vectorSetExpectedResultsProvider)
		{
			_vectorSetProvider = vectorSetProvider;
			_vectorSetExpectedResultsProvider = vectorSetExpectedResultsProvider;
		}

		public Result Cancel(long vectorSetID)
		{
			//Cancel the vector set
			return _vectorSetProvider.Cancel(vectorSetID);

			//TODO - Potentially update the Test Session status - if this was the last non-passed vector set, then the test session passes
		}

		public Result Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities)
		{
			Result result;

			//Insert the vector set record
			result = _vectorSetProvider.Insert(vectorSetID, testSessionID, generatorVersion, algorithmID);

			if (result.IsSuccess)
			{
				//Insert a vector set expected results record, which is where the capabilities actually go
				result = _vectorSetExpectedResultsProvider.InsertWithCapabilities(vectorSetID, capabilities);
			}

			return result;
		}

		public Result UpdateSubmittedResults(long vectorSetID, string results)
		{
			return _vectorSetProvider.UpdateSubmittedResults(vectorSetID, results);
		}

		public Result RecordError(long vectorSetID, string errorMessage)
		{
			return _vectorSetProvider.UpdateStatus(vectorSetID, VectorSetStatus.Error, errorMessage);
		}

		public List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetsForTestSession(long testSessionID) => _vectorSetProvider.GetVectorSetIDsForTestSession(testSessionID);

		public string GetCapabilities(long vectorSetID) => _vectorSetExpectedResultsProvider.GetCapabilities(vectorSetID);

		public TestVectorSet GetTestVectorSet(long vectorSetId)
		{
			var result = _vectorSetProvider.GetTestVectorSet(vectorSetId);
			
			if (result == null)
				return null;
			
			result.JsonFilesAvailable = _vectorSetProvider.GetTestVectorSetJsonFilesAvailable(vectorSetId);
			
			return result;
		}

		public string GetTestVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType)
		{
			return _vectorSetProvider.GetTestVectorFileJson(vectorSetId, fileType);
		}
	}
}
