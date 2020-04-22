using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.SHS
{
	public class SHA_384 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "messageLength")]
		public Domain MessageLength { get; set; } = new Domain();

		public SHA_384(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHA2-384")
		{
			bool byteOnly = options.GetValue("SHA384_Byte") == "True";

			MessageLength.Add(new Range
			{
				Min = options.GetValue("SHA_NoNull") == "True" ? (byteOnly ? 8 : 1) : 0,        //First, whether it supports 0, and if not, then if it starts at 1 or 8 depends on byteOnly
				Max = 102400,
				Increment = byteOnly ? 8 : 1
			});
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.SHA2_384
		{
			MessageLength = MessageLength.ToCoreDomain()
		};
	}
}
