using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.SHS
{
	public class SHA_256 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "messageLength")]
		public Domain MessageLength { get; set; } = new Domain();

		public SHA_256(Dictionary<string, string> options) : base("SHA2-256")
		{
			bool byteOnly = options.GetValue("SHA256_Byte") == "True";

			MessageLength.Add(new Range
			{
				Min = options.GetValue("SHA_NoNull") == "True" ? (byteOnly ? 8 : 1) : 0,        //First, whether it supports 0, and if not, then if it starts at 1 or 8 depends on byteOnly
				Max = 51200,
				Increment = byteOnly ? 8 : 1
			});
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.SHA2_256
		{
			MessageLength = MessageLength.ToCoreDomain()
		};
	}
}
