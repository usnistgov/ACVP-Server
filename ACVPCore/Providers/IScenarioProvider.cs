using System.Collections.Generic;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IScenarioProvider
	{
		InsertResult Insert(long validationID);
		List<long> GetScenarioIDsForValidation(long validationID);
	}
}