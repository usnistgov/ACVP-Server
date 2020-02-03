using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IVectorSetService
	{
		Result Cancel(long vectorSetID);
		Result UpdateSubmittedResults(long vectorSetID, string results);
		Result Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities);
		Result RecordError(long vectorSetID, string errorMessage);
		List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetsForTestSession(long testSessionID);
		string GetCapabilities(long vectorSetID);
	}
}