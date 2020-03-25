using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.KAS
{
	public class Component_KAS_ECC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Functions { get; private set; }

		[JsonProperty("scheme")]
		public SchemeCollection Schemes { get; private set; } = new SchemeCollection();

		public Component_KAS_ECC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KAS-ECC", "Component")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("ECDSA", options.GetValue("KAS_Prerequisite_ECDSA")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KAS_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("KAS_Prerequisite_DRBG")));

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

			//FullUnified
			if (options.GetValue("KASECC_FullUnified_ZZTestOnly") == "True")
			{
				Schemes.FullUnified = new FullUnified();

				//Roles
				if (options.GetValue("KASECC_FullUnified_Initiator") == "True") Schemes.FullUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_FullUnified_Responder") == "True") Schemes.FullUnified.Role.Add("responder");


				Schemes.FullUnified.NoKdfNoKc = new NoKdfNoKc();
				Schemes.FullUnified.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_FullUnified_ZZOnly", options);

			}


			//FullMQV
			if (options.GetValue("KASECC_FullMQV_ZZTestOnly") == "True")
			{
				Schemes.FullMQV = new FullMQV();

				//Roles
				if (options.GetValue("KASECC_FullMQV_Initiator") == "True") Schemes.FullMQV.Role.Add("initiator");
				if (options.GetValue("KASECC_FullMQV_Responder") == "True") Schemes.FullMQV.Role.Add("responder");


				Schemes.FullMQV.NoKdfNoKc = new NoKdfNoKc();
				Schemes.FullMQV.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_FullMQV_ZZOnly", options);

			}


			//EphemeralUnified
			if (options.GetValue("KASECC_EphemUnified_ZZTestOnly") == "True")
			{
				Schemes.EphemeralUnified = new EphemeralUnified();

				//Roles
				if (options.GetValue("KASECC_EphemUnified_Initiator") == "True") Schemes.EphemeralUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_EphemUnified_Responder") == "True") Schemes.EphemeralUnified.Role.Add("responder");

				//Ephem can't have KC, so this will have NoKdfNoKc populated
				Schemes.EphemeralUnified.NoKdfNoKc = new NoKdfNoKc();

				Schemes.EphemeralUnified.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_EphemUnified_ZZOnly", options);
			}


			//OnePassUnified
			if (options.GetValue("KASECC_OnePassUnified_ZZTestOnly") == "True")
			{
				Schemes.OnePassUnified = new OnePassUnified();

				//Roles
				if (options.GetValue("KASECC_OnePassUnified_Initiator") == "True") Schemes.OnePassUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassUnified_Responder") == "True") Schemes.OnePassUnified.Role.Add("responder");

				Schemes.OnePassUnified.NoKdfNoKc = new NoKdfNoKc();
				Schemes.OnePassUnified.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassUnified_ZZOnly", options);
			}


			//OnePassMQV
			if (options.GetValue("KASECC_OnePassMQV_ZZTestOnly") == "True")
			{
				Schemes.OnePassMQV = new OnePassMQV();

				//Roles
				if (options.GetValue("KASECC_OnePassMQV_Initiator") == "True") Schemes.OnePassMQV.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassMQV_Responder") == "True") Schemes.OnePassMQV.Role.Add("responder");

				Schemes.OnePassMQV.NoKdfNoKc = new NoKdfNoKc();
				Schemes.OnePassMQV.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassMQV_ZZOnly", options);
			}


			//OnePassDH
			if (options.GetValue("KASECC_OnePassDH_ZZTestOnly") == "True")
			{
				Schemes.OnePassDH = new OnePassDH();

				//Roles
				if (options.GetValue("KASECC_OnePassDH_Initiator") == "True") Schemes.OnePassDH.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassDH_Responder") == "True") Schemes.OnePassDH.Role.Add("responder");

				Schemes.OnePassDH.NoKdfNoKc = new NoKdfNoKc();
				Schemes.OnePassDH.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassDH_ZZOnly", options);
			}


			//StaticUnified
			if (options.GetValue("KASECC_StaticUnified_ZZTestOnly") == "True")
			{
				Schemes.StaticUnified = new StaticUnified();

				//Roles
				if (options.GetValue("KASECC_StaticUnified_Initiator") == "True") Schemes.StaticUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_StaticUnified_Responder") == "True") Schemes.StaticUnified.Role.Add("responder");

				Schemes.StaticUnified.NoKdfNoKc = new NoKdfNoKc();
				Schemes.StaticUnified.NoKdfNoKc.ParameterSets = new ParameterSets("KASECC_StaticUnified_ZZOnly", options);
			}
		}

		public class SchemeCollection
		{

			[JsonProperty("fullUnified", NullValueHandling = NullValueHandling.Ignore)]
			public FullUnified FullUnified { get; set; }

			[JsonProperty("fullMqv", NullValueHandling = NullValueHandling.Ignore)]
			public FullMQV FullMQV { get; set; }

			[JsonProperty("ephemeralUnified", NullValueHandling = NullValueHandling.Ignore)]
			public EphemeralUnified EphemeralUnified { get; set; }

			[JsonProperty("onePassUnified", NullValueHandling = NullValueHandling.Ignore)]
			public OnePassUnified OnePassUnified { get; set; }

			[JsonProperty("onePassMqv", NullValueHandling = NullValueHandling.Ignore)]
			public OnePassMQV OnePassMQV { get; set; }

			[JsonProperty("onePassDh", NullValueHandling = NullValueHandling.Ignore)]
			public OnePassDH OnePassDH { get; set; }

			[JsonProperty("staticUnified", NullValueHandling = NullValueHandling.Ignore)]
			public StaticUnified StaticUnified { get; set; }

			public NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.SchemeCollection ToPersisted() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.SchemeCollection
			{
				EphemeralUnified = EphemeralUnified?.ToPersisted(),
				FullUnified = FullUnified?.ToPersisted(),
				FullMQV = FullMQV?.ToPersisted(),
				OnePassUnified = OnePassUnified?.ToPersisted(),
				OnePassMQV = OnePassMQV?.ToPersisted(),
				OnePassDH = OnePassDH?.ToPersisted(),
				StaticUnified = StaticUnified?.ToPersisted()
			};
		}

		public abstract class SchemeBase
		{
			[JsonProperty("kasRole")]
			public List<string> Role { get; set; } = new List<string>();

			[JsonProperty("noKdfNoKc", NullValueHandling = NullValueHandling.Ignore)]
			public NoKdfNoKc NoKdfNoKc { get; set; }

			public NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.Scheme ToPersisted() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.Scheme
			{
				Role = Role,
				NoKdfNoKc = NoKdfNoKc?.ToPersisted()
			};
		}


		public class EphemeralUnified : SchemeBase { }


		public class OnePassMQV : SchemeBase { }


		public class FullUnified : SchemeBase { }


		public class FullMQV : SchemeBase { }


		public class OnePassUnified : SchemeBase { }


		public class OnePassDH : SchemeBase { }


		public class StaticUnified : SchemeBase { }



		public class NoKdfNoKc
		{
			[JsonProperty("parameterSet")]
			public ParameterSets ParameterSets { get; set; }

			public NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.NoKdfNoKc ToPersisted() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.NoKdfNoKc
			{
				ParameterSets = ParameterSets?.ToPersisted()
			};
		}


		public class ParameterSets
		{
			[JsonProperty("eb", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSet EB { get; set; }

			[JsonProperty("ec", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSet EC { get; set; }

			[JsonProperty("ed", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSet ED { get; set; }

			[JsonProperty("ee", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSet EE { get; set; }

			public ParameterSets(string optionPrefix, Dictionary<string, string> options)
			{
				if (options.GetValue($"{optionPrefix}_EB") == "True") EB = new ParameterSet(optionPrefix, "EB", options);
				if (options.GetValue($"{optionPrefix}_EC") == "True") EC = new ParameterSet(optionPrefix, "EC", options);
				if (options.GetValue($"{optionPrefix}_ED") == "True") ED = new ParameterSet(optionPrefix, "ED", options);
				if (options.GetValue($"{optionPrefix}_EE") == "True") EE = new ParameterSet(optionPrefix, "EE", options);
			}

			public NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.ParameterSets ToPersisted() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.ParameterSets
			{
				EB = EB?.ToPersisted(),
				EC = EC?.ToPersisted(),
				ED = ED?.ToPersisted(),
				EE = EE?.ToPersisted()
			};
		}


		public class ParameterSet
		{
			[JsonProperty("curve")]
			public string Curve { get; set; }

			[JsonProperty("hashAlg")]
			public List<string> HashAlg { get; set; } = new List<string>();

			public ParameterSet(string optionPrefix, string parameterSetName, Dictionary<string, string> options)
			{
				List<string> curves;
				List<int> hashLen;

				switch (parameterSetName)
				{
					case "EB":
						curves = new List<string> { "P224", "K233", "B233" };
						hashLen = new List<int> { 224, 256, 384, 512 };
						break;
					case "EC":
						curves = new List<string> { "P256", "K283", "B283" };
						hashLen = new List<int> { 256, 384, 512 };
						break;
					case "ED":
						curves = new List<string> { "P384", "K409", "B409" };
						hashLen = new List<int> { 384, 512 };
						break;
					case "EE":
						curves = new List<string> { "P521", "K571", "B571" };
						hashLen = new List<int> { 512 };
						break;
					default:
						curves = new List<string>();
						hashLen = new List<int>();
						break;
				}

				//Curve
				foreach (string curve in curves)
				{
					if (options.GetValue($"{optionPrefix}_{parameterSetName}_Curve{curve}") == "True")
					{
						Curve = curve.Insert(1, "-");   //Need to inject the - into the curve name
						break;      //Should only be 1, so will stop on the first
					}
				}

				//Hash Algs
				foreach (int len in hashLen)
				{
					if (options.GetValue($"{optionPrefix}_{parameterSetName}_SHA{len}") == "True") HashAlg.Add($"SHA2-{len}");
				}
			}

			public NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.ParameterSet ToPersisted() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component.ParameterSet
			{
				Curve = Curve,
				HashAlg = HashAlg
			};
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.KAS_ECC_Component
		{
			Functions = Functions,
			Schemes = Schemes?.ToPersisted()
		};
	}
}
