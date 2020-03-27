using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.SHS
{
	public class SHA_512256 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "messageLength")]
		public Domain MessageLength { get; set; } = new Domain();

		public SHA_512256(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHA2-512/256")
		{
			bool byteOnly = options.GetValue("SHA512_256_Byte") == "True";

			MessageLength.Add(new Range
			{
				Min = options.GetValue("SHA_NoNull") == "True" ? (byteOnly ? 8 : 1) : 0,        //First, whether it supports 0, and if not, then if it starts at 1 or 8 depends on byteOnly
				Max = 102400,
				Increment = byteOnly ? 8 : 1
			});
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.SHA2_512256
		{
			MessageLength = MessageLength.ToCoreDomain()
		};
	}
}
