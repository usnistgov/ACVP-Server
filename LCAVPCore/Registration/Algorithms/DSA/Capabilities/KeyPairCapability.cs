using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.DSA.Capabilities
{
	public class KeyPairCapability
	{
		[JsonProperty(PropertyName = "l")]
		public int L { get; private set; }

		[JsonProperty(PropertyName = "n")]
		public int N { get; private set; }

		public KeyPairCapability(int l, int n)
		{
			L = l;
			N = n;
		}
	}
}
