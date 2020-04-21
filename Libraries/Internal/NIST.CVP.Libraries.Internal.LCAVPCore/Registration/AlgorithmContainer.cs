using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class AlgorithmContainer
	{
		[JsonProperty(PropertyName = "name", Order = 1)]
		public string Algorithm { get; set; }

		[JsonProperty(PropertyName = "mode", Order = 2)]
		public string Mode { get; set; }

		[JsonProperty(PropertyName = "revision", Order = 3)]
		public string Revision { get; set; }
	}
}
