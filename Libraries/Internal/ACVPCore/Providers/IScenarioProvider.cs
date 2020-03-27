using System.Collections.Generic;
using NIST.CVP.Results;


namespace ACVPCore.Providers
{
	public interface IScenarioProvider
	{
		InsertResult Insert(long validationID);
		long GetScenarioIDForValidationOE(long validationID, long oeID);
	}
}