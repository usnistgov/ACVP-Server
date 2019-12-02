using ACVPCore.Providers;

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

		public void Cancel(long vectorSetID)
		{
			//Cancel the vector set
			_vectorSetProvider.Cancel(vectorSetID);
		}

		public void Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities)
		{
			//Insert the vector set record
			_vectorSetProvider.Insert(vectorSetID, testSessionID, generatorVersion, algorithmID);

			//Insert a vector set expected results record, which is where the capabilities actually go
			_vectorSetExpectedResultsProvider.InsertWithCapabilities(vectorSetID, capabilities);
		}

		public void UpdateSubmittedResults(long vectorSetID, string results)
		{
			_vectorSetProvider.UpdateSubmittedResults(vectorSetID, results);
		}
	}
}
