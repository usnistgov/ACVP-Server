using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.KAS
{
	public class ECC_CDH : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Functions { get; private set; }

		[JsonProperty("curve")]
		public List<string> Curves { get; private set; } = new List<string>();

		public ECC_CDH(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KAS-ECC", "CDH-Component")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("ECDSA", options.GetValue("KAS_Prerequisite_ECDSA")));

			//Functions
			List<string> functions = new List<string>();
			if (options.GetValue("KASECC_KAS_Assurance_ElementDPG") == "True") functions.Add("dpGen");
			if (options.GetValue("KASECC_KAS_Assurance_ElementDPV") == "True") functions.Add("dpVal");
			if (options.GetValue("KASECC_KAS_Assurance_ElementKPG") == "True") functions.Add("keyPairGen");
			if (options.GetValue("KASECC_KAS_Assurance_ElementFV") == "True") functions.Add("fullVal");
			if (options.GetValue("KASECC_KAS_Assurance_ElementKR") == "True") functions.Add("keyRegen");
			if (options.GetValue("KASECC_KAS_Assurance_ElementPV") == "True") functions.Add("partialVal");
			if (functions.Count > 0)
			{
				Functions = functions;
			}

			//Curves
			if (options.GetValue("KASECC_Comp_DLC_Prim_P224") == "True") Curves.Add("P-224");
			if (options.GetValue("KASECC_Comp_DLC_Prim_P256") == "True") Curves.Add("P-256");
			if (options.GetValue("KASECC_Comp_DLC_Prim_P384") == "True") Curves.Add("P-384");
			if (options.GetValue("KASECC_Comp_DLC_Prim_P521") == "True") Curves.Add("P-521");
			if (options.GetValue("KASECC_Comp_DLC_Prim_K233") == "True") Curves.Add("K-233");
			if (options.GetValue("KASECC_Comp_DLC_Prim_K283") == "True") Curves.Add("K-283");
			if (options.GetValue("KASECC_Comp_DLC_Prim_K409") == "True") Curves.Add("K-409");
			if (options.GetValue("KASECC_Comp_DLC_Prim_K571") == "True") Curves.Add("K-571");
			if (options.GetValue("KASECC_Comp_DLC_Prim_B233") == "True") Curves.Add("B-233");
			if (options.GetValue("KASECC_Comp_DLC_Prim_B283") == "True") Curves.Add("B-283");
			if (options.GetValue("KASECC_Comp_DLC_Prim_B409") == "True") Curves.Add("B-409");
			if (options.GetValue("KASECC_Comp_DLC_Prim_B571") == "True") Curves.Add("B-571");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.KAS_ECC_CDH
		{
			Functions = Functions,
			Curves = Curves
		};

	}
}
