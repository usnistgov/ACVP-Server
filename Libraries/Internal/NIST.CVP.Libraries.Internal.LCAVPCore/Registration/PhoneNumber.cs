using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class PhoneNumber
	{
		[JsonProperty("number")]
		public string Number { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
