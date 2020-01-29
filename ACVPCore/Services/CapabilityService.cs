using System.Collections.Generic;
using System.Linq;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class CapabilityService : ICapabilityService
	{
		ICapabilityProvider _capabilityProvider;

		public CapabilityService(ICapabilityProvider capabilityProvider)
		{
			_capabilityProvider = capabilityProvider;
		}
	}
}
