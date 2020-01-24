using System.Collections.Generic;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Services
{
	public interface ICapabilityService
	{
		List<ICapability> GetCapabilitiesForComparison(long scenarioAlgorithmId);
	}
}