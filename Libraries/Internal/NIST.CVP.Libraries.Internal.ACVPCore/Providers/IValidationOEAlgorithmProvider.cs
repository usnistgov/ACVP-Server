using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IValidationOEAlgorithmProvider
	{
		InsertResult Insert(long validationID, long oeID, long algorithmID, long vectorSetID);
		Result Inactivate(long validationOEAlgorithmID);
		List<ValidationOEAlgorithmDisplay> GetActiveValidationOEAlgorithmsForDisplay(long validationID);
		List<(long ValidationOEAlgorithmID, long AlgorithmID)> GetActiveValidationOEAlgorithms(long validationID, long oeID);
	}
}