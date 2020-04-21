using System;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface ICapabilityService
	{
		Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID);
		void CreateClassCapabilities(long algorithmID, long scenarioAlgorithmID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string parentPropertyName, Object objectClass);
	}
}