using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IScenarioProvider
	{
		InsertResult Insert(long validationID);
		long GetScenarioIDForValidationOE(long validationID, long oeID);
	}
}