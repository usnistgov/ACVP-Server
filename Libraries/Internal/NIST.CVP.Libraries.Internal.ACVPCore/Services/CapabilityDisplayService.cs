using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class CapabilityDisplayService : ICapabilityDisplayService
	{
		private readonly ICapabilityProvider _capabilityProvider;

		public CapabilityDisplayService(ICapabilityProvider capabilityProvider)
		{
			_capabilityProvider = capabilityProvider;
		}

		public CapabilitiesDisplay GetCapabilitiesDisplay(long validationOEAlgorithmID)
		{
			//Get the capabilities
			List<RawCapability> rawCapabilities = _capabilityProvider.GetCapabilities(validationOEAlgorithmID);

			if (rawCapabilities.Count == 0)
			{
				return null;
			}

			//Start building...
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<ul class=\"list list-unstyled\">");

			foreach (RawCapability rawCapability in rawCapabilities.Where(x => x.ParentCapabilityID == null).OrderBy(x => x.PropertyOrderIndex).ThenBy(x => x.PropertyDisplayName))
			{
				string capabilityHTML = CapabilityFactory.BuildCapability(rawCapability, rawCapabilities).HTML;
				if (!string.IsNullOrEmpty(capabilityHTML))
				{
					stringBuilder.Append("<li>");
					stringBuilder.Append(capabilityHTML);
					stringBuilder.Append("</li>");
				}
			}
			stringBuilder.Append("</ul>");
			return new CapabilitiesDisplay { HTML = stringBuilder.ToString() };
		}
	}
}
