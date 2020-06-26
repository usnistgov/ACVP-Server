using NIST.CVP.Libraries.Internal.ACVPCore.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface ICapabilityDisplayService
	{
		CapabilitiesDisplay GetCapabilitiesDisplay(long validationOEAlgorithmID);
	}
}