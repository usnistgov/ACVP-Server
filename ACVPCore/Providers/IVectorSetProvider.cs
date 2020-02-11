using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IVectorSetProvider
	{
		Result Cancel(long id);
		Result UpdateSubmittedResults(long vectorSetID, string results);
		Result UpdateStatus(long vectorSetID, VectorSetStatus status, string errorMessage);
		Result Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID);
		List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetIDsForTestSession(long testSessionID);
	}
}