using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IScenarioProvider
	{
		InsertResult Insert(long validationID);
		long GetScenarioIDForValidationOE(long validationID, long oeID);
	}
}