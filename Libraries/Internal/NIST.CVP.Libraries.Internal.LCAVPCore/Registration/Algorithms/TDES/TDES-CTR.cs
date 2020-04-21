using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CTR : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get => new List<string> { "encrypt" }; }    //CAVS doesn't allow decrypt...

		//ctrSource isn't something we care about going forward
		//[JsonProperty(PropertyName = "ctrSource")]
		//public string CtrSource { get; private set; }

		public TDES_CTR(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-TDES-CTR")
		{
			//if (options.GetValue("CTR_Src") == "External" || options.GetValue("CTR_Src") == "Both") Direction.Add("external");
			//if (options.GetValue("CTR_Src") == "Internal" || options.GetValue("CTR_Src") == "Both") Direction.Add("internal");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.TDES_CTR
		{
			Direction = Direction,
		};
	}
}
