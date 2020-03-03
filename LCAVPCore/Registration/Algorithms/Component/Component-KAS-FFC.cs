using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class Component_KAS_FFC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Functions { get; private set; }

		[JsonProperty("scheme")]
		public Schemes Schemes { get; private set; } = new Schemes();

		public Component_KAS_FFC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KAS-FFC", "Component")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("DSA", options.GetValue("KAS_Prerequisite_DSA")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KAS_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("KAS_Prerequisite_DRBG")));

			//Functions - There is a 6th, PV, in the data, but it is for ECC only
			List<string> functions = new List<string>();
			if (options.GetValue("KASFFC_KAS_Assurance_ElementDPG") == "True") functions.Add("dpGen");
			if (options.GetValue("KASFFC_KAS_Assurance_ElementDPV") == "True") functions.Add("dpVal");
			if (options.GetValue("KASFFC_KAS_Assurance_ElementKPG") == "True") functions.Add("keyPairGen");
			if (options.GetValue("KASFFC_KAS_Assurance_ElementFV") == "True") functions.Add("fullVal");
			if (options.GetValue("KASFFC_KAS_Assurance_ElementKR") == "True") functions.Add("keyRegen");
			if (functions.Count > 0)
			{
				Functions = functions;
			}

			//Hybrid1
			if (options.GetValue("KASFFC_Hybrid1_ZZTestOnly") == "True")
			{
				Schemes.DhHybrid1 = new DhHybrid1();

				//Roles
				if (options.GetValue("KASFFC_Hybrid1_Initiator") == "True") Schemes.DhHybrid1.Role.Add("initiator");
				if (options.GetValue("KASFFC_Hybrid1_Responder") == "True") Schemes.DhHybrid1.Role.Add("responder");

				Schemes.DhHybrid1.NoKdfNoKc = new NoKdfNoKc();
				Schemes.DhHybrid1.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1_ZZOnly", options);
			}


			//MQV2
			if (options.GetValue("KASFFC_MQV2_ZZTestOnly") == "True")
			{
				Schemes.Mqv2 = new Mqv2();

				//Roles
				if (options.GetValue("KASFFC_MQV2_Initiator") == "True") Schemes.Mqv2.Role.Add("initiator");
				if (options.GetValue("KASFFC_MQV2_Responder") == "True") Schemes.Mqv2.Role.Add("responder");

				Schemes.Mqv2.NoKdfNoKc = new NoKdfNoKc();
				Schemes.Mqv2.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_MQV2_ZZOnly", options);
			}


			//dhEphem
			if (options.GetValue("KASFFC_Ephem_ZZTestOnly") == "True")
			{
				Schemes.DhEphem = new DhEphem();

				//Roles
				if (options.GetValue("KASFFC_Ephem_Initiator") == "True") Schemes.DhEphem.Role.Add("initiator");
				if (options.GetValue("KASFFC_Ephem_Responder") == "True") Schemes.DhEphem.Role.Add("responder");

				Schemes.DhEphem.NoKdfNoKc = new NoKdfNoKc();
				Schemes.DhEphem.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_Ephem_ZZOnly", options);
			}


			//dhHybridOneFlow
			if (options.GetValue("KASFFC_Hybrid1Flow_ZZTestOnly") == "True")
			{
				Schemes.DhHybridOneFlow = new DhHybridOneFlow();

				//Roles
				if (options.GetValue("KASFFC_Hybrid1Flow_Initiator") == "True") Schemes.DhHybridOneFlow.Role.Add("initiator");
				if (options.GetValue("KASFFC_Hybrid1Flow_Responder") == "True") Schemes.DhHybridOneFlow.Role.Add("responder");

				Schemes.DhHybridOneFlow.NoKdfNoKc = new NoKdfNoKc();
				Schemes.DhHybridOneFlow.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1Flow_ZZOnly", options);
			}


			//MQV1
			if (options.GetValue("KASFFC_MQV1_ZZTestOnly") == "True")
			{
				Schemes.Mqv1 = new Mqv1();

				//Roles
				if (options.GetValue("KASFFC_MQV1_Initiator") == "True") Schemes.Mqv1.Role.Add("initiator");
				if (options.GetValue("KASFFC_MQV1_Responder") == "True") Schemes.Mqv1.Role.Add("responder");

				Schemes.Mqv1.NoKdfNoKc = new NoKdfNoKc();
				Schemes.Mqv1.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_MQV1_ZZOnly", options);
			}


			//dhOneFlow
			if (options.GetValue("KASFFC_OneFlow_ZZTestOnly") == "True")
			{
				Schemes.DhOneFlow = new DhOneFlow();

				//Roles
				if (options.GetValue("KASFFC_OneFlow_Initiator") == "True") Schemes.DhOneFlow.Role.Add("initiator");
				if (options.GetValue("KASFFC_OneFlow_Responder") == "True") Schemes.DhOneFlow.Role.Add("responder");

				Schemes.DhOneFlow.NoKdfNoKc = new NoKdfNoKc();
				Schemes.DhOneFlow.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_OneFlow_ZZOnly", options);
			}


			//dhStatic
			if (options.GetValue("KASFFC_Static_ZZTestOnly") == "True")
			{
				Schemes.DhStatic = new DhStatic();

				//Roles
				if (options.GetValue("KASFFC_Static_Initiator") == "True") Schemes.DhStatic.Role.Add("initiator");
				if (options.GetValue("KASFFC_Static_Responder") == "True") Schemes.DhStatic.Role.Add("responder");

				Schemes.DhStatic.NoKdfNoKc = new NoKdfNoKc();
				Schemes.DhStatic.NoKdfNoKc.ParameterSets = new ParameterSets("KASFFC_Static_ZZOnly", options);
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component
		{
			Functions = Functions,
			Schemes = Schemes.ToPersisted()
		};
	}

	public class Schemes
	{
		[JsonProperty("dhEphem", NullValueHandling = NullValueHandling.Ignore)]
		public DhEphem DhEphem { get; set; }

		[JsonProperty("mqv1", NullValueHandling = NullValueHandling.Ignore)]
		public Mqv1 Mqv1 { get; set; }

		[JsonProperty("dhHybrid1", NullValueHandling = NullValueHandling.Ignore)]
		public DhHybrid1 DhHybrid1 { get; set; }

		[JsonProperty("mqv2", NullValueHandling = NullValueHandling.Ignore)]
		public Mqv2 Mqv2 { get; set; }

		[JsonProperty("hybridOneFlow", NullValueHandling = NullValueHandling.Ignore)]
		public DhHybridOneFlow DhHybridOneFlow { get; set; }

		[JsonProperty("dhOneFlow", NullValueHandling = NullValueHandling.Ignore)]
		public DhOneFlow DhOneFlow { get; set; }

		[JsonProperty("dhStatic", NullValueHandling = NullValueHandling.Ignore)]
		public DhStatic DhStatic { get; set; }

		public ACVPCore.Algorithms.Persisted.KAS_FFC_Component.SchemeCollection ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component.SchemeCollection
		{
			DhEphem = DhEphem.ToPersisted(),
			Mqv1 = Mqv1.ToPersisted(),
			DhHybrid1 = DhHybrid1.ToPersisted(),
			Mqv2 = Mqv2.ToPersisted(),
			DhHybridOneFlow = DhHybridOneFlow.ToPersisted(),
			DhOneFlow = DhOneFlow.ToPersisted(),
			DhStatic = DhStatic.ToPersisted()
		};
	}

	public abstract class Scheme
	{
		[JsonProperty("kasRole")]
		public List<string> Role { get; set; } = new List<string>();

		[JsonProperty("noKdfNoKc", NullValueHandling = NullValueHandling.Ignore)]
		public NoKdfNoKc NoKdfNoKc { get; set; }

		public ACVPCore.Algorithms.Persisted.KAS_FFC_Component.Scheme ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component.Scheme
		{
			Role = Role,
			NoKdfNoKc = NoKdfNoKc.ToPersisted()
		};
	}


	public class DhEphem : Scheme { }


	public class Mqv1 : Scheme { }


	public class DhHybrid1 : Scheme { }


	public class Mqv2 : Scheme { }


	public class DhHybridOneFlow : Scheme { }


	public class DhOneFlow : Scheme { }


	public class DhStatic : Scheme { }



	public class NoKdfNoKc
	{
		[JsonProperty("parameterSet")]
		public ParameterSets ParameterSets { get; set; }

		public ACVPCore.Algorithms.Persisted.KAS_FFC_Component.NoKdfNoKc ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component.NoKdfNoKc
		{
			ParameterSets = ParameterSets.ToPersisted()
		};
	}


	public class ParameterSets
	{
		[JsonProperty("fb", NullValueHandling = NullValueHandling.Ignore)]
		public ParameterSet Fb { get; set; }

		[JsonProperty("fc", NullValueHandling = NullValueHandling.Ignore)]
		public ParameterSet Fc { get; set; }

		public ParameterSets(string optionPrefix, Dictionary<string, string> options)
		{
			//Build the FB and FC parameter sets - FA is in the inf but can be ignored
			if (options.GetValue($"{optionPrefix}_FB") == "True") Fb = new ParameterSet(optionPrefix, "FB", options);
			if (options.GetValue($"{optionPrefix}_FC") == "True") Fc = new ParameterSet(optionPrefix, "FC", options);
		}

		public ACVPCore.Algorithms.Persisted.KAS_FFC_Component.ParameterSets ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component.ParameterSets
		{
			FB = Fb.ToPersisted(),
			FC = Fc.ToPersisted()
		};
	}


	public class ParameterSet
	{
		[JsonProperty("hashAlg")]
		public List<string> HashAlg { get; set; } = new List<string>();

		public ParameterSet(string optionPrefix, string parameterSetName, Dictionary<string, string> options)
		{
			//Possible lengths depends on the parameter set - FB has 224, otherwise they're the same
			int[] shaLens = parameterSetName == "FB" ? new int[] { 224, 256, 384, 512 } : new int[] { 256, 384, 512 };

			//Hash Algs
			foreach (int len in shaLens)
			{
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_SHA{len}") == "True") HashAlg.Add($"SHA2-{len}");
			}
		}

		public ACVPCore.Algorithms.Persisted.KAS_FFC_Component.ParameterSet ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC_Component.ParameterSet
		{
			HashAlg = HashAlg
		};
	}
}

