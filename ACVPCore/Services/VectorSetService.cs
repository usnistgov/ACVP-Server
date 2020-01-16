using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class VectorSetService : IVectorSetService
	{
		IVectorSetProvider _vectorSetProvider;
		IVectorSetExpectedResultsProvider _vectorSetExpectedResultsProvider;

		public VectorSetService(IVectorSetProvider vectorSetProvider, IVectorSetExpectedResultsProvider vectorSetExpectedResultsProvider)
		{
			_vectorSetProvider = vectorSetProvider;
			_vectorSetExpectedResultsProvider = vectorSetExpectedResultsProvider;
		}

		public Result Cancel(long vectorSetID)
		{
			//Cancel the vector set
			return _vectorSetProvider.Cancel(vectorSetID);
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
	}
}
