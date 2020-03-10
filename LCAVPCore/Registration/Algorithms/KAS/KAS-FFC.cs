using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.KAS
{
	public class KAS_FFC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Functions { get; private set; }

		[JsonProperty("scheme")]
		public SchemeCollection Schemes { get; private set; } = new SchemeCollection();

		public KAS_FFC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KAS-FFC")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("DSA", options.GetValue("KAS_Prerequisite_DSA")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KAS_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("KAS_Prerequisite_DRBG")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KAS_Prerequisite_CCM")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KAS_Prerequisite_CMAC")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KAS_Prerequisite_HMAC")));

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

			//The general logic for determining the KDF and KC "modes", repeated within each of the schemes
			//ZZTestOnly = True --> NoKdfNoKc
			//ZZTestOnly = False --> KeyConfirm = True --> KdfKc
			//					 --> KeyConfirm = False --> KdfNoKc

			//Hybrid1
			if (options.GetValue("KASFFC_Hybrid1") == "True" && options.GetValue("KASFFC_Hybrid1_ZZTestOnly") != "True")
			{
				Schemes.DhHybrid1 = new DhHybrid1();

				//Roles
				if (options.GetValue("KASFFC_Hybrid1_Initiator") == "True") Schemes.DhHybrid1.Role.Add("initiator");
				if (options.GetValue("KASFFC_Hybrid1_Responder") == "True") Schemes.DhHybrid1.Role.Add("responder");

				if (options.GetValue("KASFFC_Hybrid1_KeyConfirm") == "True")
				{
					Schemes.DhHybrid1.KdfKc = new KdfKc();
					Schemes.DhHybrid1.KdfKc.KdfOption = new KdfOptions("KASFFC_Hybrid1", options);
					Schemes.DhHybrid1.KdfKc.KcOption = new KcOptions("KASFFC_Hybrid1", options);
					Schemes.DhHybrid1.KdfKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1_KC", options);
				}
				else
				{
					Schemes.DhHybrid1.KdfNoKc = new KdfNoKc();
					Schemes.DhHybrid1.KdfNoKc.KdfOption = new KdfOptions("KASFFC_Hybrid1", options);
					Schemes.DhHybrid1.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1_NOKC", options);
				}
			}


			//MQV2
			if (options.GetValue("KASFFC_MQV2") == "True" && options.GetValue("KASFFC_MQV2_ZZTestOnly") != "True")
			{
				Schemes.Mqv2 = new Mqv2();

				//Roles
				if (options.GetValue("KASFFC_MQV2_Initiator") == "True") Schemes.Mqv2.Role.Add("initiator");
				if (options.GetValue("KASFFC_MQV2_Responder") == "True") Schemes.Mqv2.Role.Add("responder");

				if (options.GetValue("KASFFC_MQV2_KeyConfirm") == "True")
				{
					Schemes.Mqv2.KdfKc = new KdfKc();
					Schemes.Mqv2.KdfKc.KdfOption = new KdfOptions("KASFFC_MQV2", options);
					Schemes.Mqv2.KdfKc.KcOption = new KcOptions("KASFFC_MQV2", options);
					Schemes.Mqv2.KdfKc.ParameterSets = new ParameterSets("KASFFC_MQV2_KC", options);
				}
				else
				{
					Schemes.Mqv2.KdfNoKc = new KdfNoKc();
					Schemes.Mqv2.KdfNoKc.KdfOption = new KdfOptions("KASFFC_MQV2", options);
					Schemes.Mqv2.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_MQV2_NOKC", options);
				}
			}


			//dhEphem
			if (options.GetValue("KASFFC_Ephem") == "True" && options.GetValue("KASFFC_Ephem_ZZTestOnly") != "True")
			{
				Schemes.DhEphem = new DhEphem();

				//Roles
				if (options.GetValue("KASFFC_Ephem_Initiator") == "True") Schemes.DhEphem.Role.Add("initiator");
				if (options.GetValue("KASFFC_Ephem_Responder") == "True") Schemes.DhEphem.Role.Add("responder");

				//Ephem can't have KC, so this will have KdfNoKc populated
				Schemes.DhEphem.KdfNoKc = new KdfNoKc();
				Schemes.DhEphem.KdfNoKc.KdfOption = new KdfOptions("KASFFC_Ephem", options);
				Schemes.DhEphem.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_Ephem_NOKC", options);
			}


			//dhHybridOneFlow
			if (options.GetValue("KASFFC_Hybrid1Flow") == "True" && options.GetValue("KASFFC_Hybrid1Flow_ZZTestOnly") != "True")
			{
				Schemes.DhHybridOneFlow = new DhHybridOneFlow();

				//Roles
				if (options.GetValue("KASFFC_Hybrid1Flow_Initiator") == "True") Schemes.DhHybridOneFlow.Role.Add("initiator");
				if (options.GetValue("KASFFC_Hybrid1Flow_Responder") == "True") Schemes.DhHybridOneFlow.Role.Add("responder");

				if (options.GetValue("KASFFC_Hybrid1Flow_KeyConfirm") == "True")
				{
					Schemes.DhHybridOneFlow.KdfKc = new KdfKc();
					Schemes.DhHybridOneFlow.KdfKc.KdfOption = new KdfOptions("KASFFC_Hybrid1Flow", options);
					Schemes.DhHybridOneFlow.KdfKc.KcOption = new KcOptions("KASFFC_Hybrid1Flow", options);
					Schemes.DhHybridOneFlow.KdfKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1Flow_KC", options);
				}
				else
				{
					Schemes.DhHybridOneFlow.KdfNoKc = new KdfNoKc();
					Schemes.DhHybridOneFlow.KdfNoKc.KdfOption = new KdfOptions("KASFFC_Hybrid1Flow", options);
					Schemes.DhHybridOneFlow.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_Hybrid1Flow_NOKC", options);
				}
			}


			//MQV1
			if (options.GetValue("KASFFC_MQV1") == "True" && options.GetValue("KASFFC_MQV1_ZZTestOnly") != "True")
			{
				Schemes.Mqv1 = new Mqv1();

				//Roles
				if (options.GetValue("KASFFC_MQV1_Initiator") == "True") Schemes.Mqv1.Role.Add("initiator");
				if (options.GetValue("KASFFC_MQV1_Responder") == "True") Schemes.Mqv1.Role.Add("responder");

				if (options.GetValue("KASFFC_MQV1_KeyConfirm") == "True")
				{
					Schemes.Mqv1.KdfKc = new KdfKc();
					Schemes.Mqv1.KdfKc.KdfOption = new KdfOptions("KASFFC_MQV1", options);
					Schemes.Mqv1.KdfKc.KcOption = new KcOptions("KASFFC_MQV1", options);
					Schemes.Mqv1.KdfKc.ParameterSets = new ParameterSets("KASFFC_MQV1_KC", options);
				}
				else
				{
					Schemes.Mqv1.KdfNoKc = new KdfNoKc();
					Schemes.Mqv1.KdfNoKc.KdfOption = new KdfOptions("KASFFC_MQV1", options);
					Schemes.Mqv1.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_MQV1_NOKC", options);
				}
			}


			//dhOneFlow
			if (options.GetValue("KASFFC_OneFlow") == "True" && options.GetValue("KASFFC_OneFlow_ZZTestOnly") != "True")
			{
				Schemes.DhOneFlow = new DhOneFlow();

				//Roles
				if (options.GetValue("KASFFC_OneFlow_Initiator") == "True") Schemes.DhOneFlow.Role.Add("initiator");
				if (options.GetValue("KASFFC_OneFlow_Responder") == "True") Schemes.DhOneFlow.Role.Add("responder");

				if (options.GetValue("KASFFC_OneFlow_KeyConfirm") == "True")
				{
					Schemes.DhOneFlow.KdfKc = new KdfKc();
					Schemes.DhOneFlow.KdfKc.KdfOption = new KdfOptions("KASFFC_OneFlow", options);
					Schemes.DhOneFlow.KdfKc.KcOption = new KcOptions("KASFFC_OneFlow", options);        //This will be kind of wrong, as OneFlow implies certain rules for the Roles and Type. Those will be set properly in subsequent code, but the nonce stuff here is still good
					Schemes.DhOneFlow.KdfKc.ParameterSets = new ParameterSets("KASFFC_OneFlow_KC", options);

					//Special OneFlow KcOption rules
					//KC roles depend on initiator/responder
					if (options.GetValue("KASFFC_OneFlow_Initiator") == "True") Schemes.DhOneFlow.KdfKc.KcOption.KcRole.Add("recipient");
					if (options.GetValue("KASFFC_OneFlow_Responder") == "True") Schemes.DhOneFlow.KdfKc.KcOption.KcRole.Add("provider");

					//Can't do bilateral, so must be unilateral
					Schemes.DhOneFlow.KdfKc.KcOption.KcType.Add("unilateral");
				}
				else
				{
					Schemes.DhOneFlow.KdfNoKc = new KdfNoKc();
					Schemes.DhOneFlow.KdfNoKc.KdfOption = new KdfOptions("KASFFC_OneFlow", options);
					Schemes.DhOneFlow.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_OneFlow_NOKC", options);
				}
			}


			//dhStatic
			if (options.GetValue("KASFFC_Static") == "True" && options.GetValue("KASFFC_Static_ZZTestOnly") != "True")
			{
				Schemes.DhStatic = new DhStatic();

				//Roles
				if (options.GetValue("KASFFC_Static_Initiator") == "True") Schemes.DhStatic.Role.Add("initiator");
				if (options.GetValue("KASFFC_Static_Responder") == "True") Schemes.DhStatic.Role.Add("responder");

				if (options.GetValue("KASFFC_Static_KeyConfirm") == "True")
				{
					Schemes.DhStatic.KdfKc = new KdfKc();
					Schemes.DhStatic.KdfKc.KdfOption = new KdfOptions("KASFFC_Static", options);
					Schemes.DhStatic.KdfKc.KcOption = new KcOptions("KASFFC_Static", options);
					Schemes.DhStatic.KdfKc.ParameterSets = new ParameterSets("KASFFC_Static_KC", options);
					Schemes.DhStatic.KdfKc.DkmNonceTypes = BuildDkmNonceTypes(options);
				}
				else
				{
					Schemes.DhStatic.KdfNoKc = new KdfNoKc();
					Schemes.DhStatic.KdfNoKc.KdfOption = new KdfOptions("KASFFC_Static", options);
					Schemes.DhStatic.KdfNoKc.ParameterSets = new ParameterSets("KASFFC_Static_NOKC", options);
					Schemes.DhStatic.KdfNoKc.DkmNonceTypes = BuildDkmNonceTypes(options);
				}
			}
		}

		private List<string> BuildDkmNonceTypes(Dictionary<string, string> options)
		{
			List<string> result = new List<string>();
			if (options.GetValue("KASFFC_Static_StaticNonce1") == "True") result.Add("randomNonce");
			if (options.GetValue("KASFFC_Static_StaticNonce2") == "True") result.Add("timestamp");
			if (options.GetValue("KASFFC_Static_StaticNonce3") == "True") result.Add("sequence");
			if (options.GetValue("KASFFC_Static_StaticNonce4") == "True") result.Add("timestampSequence");
			return result.Count > 0 ? result : null;
		}

		public class SchemeCollection
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

			public ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeCollection ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeCollection
			{
				DhEphem = DhEphem?.ToPersistedNoKC(),
				Mqv1 = Mqv1?.ToPersisted(),
				DhHybrid1 = DhHybrid1?.ToPersisted(),
				Mqv2 = Mqv2?.ToPersisted(),
				DhHybridOneFlow = DhHybridOneFlow?.ToPersisted(),
				DhOneFlow = DhOneFlow?.ToPersisted(),
				DhStatic = DhStatic?.ToPersistedDhStatic()
			};
		}

		public abstract class SchemeBase
		{
			[JsonProperty("kasRole")]
			public List<string> Role { get; set; } = new List<string>();

			[JsonProperty("noKdfNoKc", NullValueHandling = NullValueHandling.Ignore)]
			public NoKdfNoKc NoKdfNoKc { get; set; }

			[JsonProperty("kdfNoKc", NullValueHandling = NullValueHandling.Ignore)]
			public KdfNoKc KdfNoKc { get; set; }

			[JsonProperty("kdfKc", NullValueHandling = NullValueHandling.Ignore)]
			public KdfKc KdfKc { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeKC ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeKC
			{
				Role = Role,
				KdfNoKc = KdfNoKc?.ToPersisted(),
				KdfKc = KdfKc?.ToPersisted()
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeNoKC ToPersistedNoKC() => new ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeNoKC
			{
				Role = Role,
				KdfNoKc = KdfNoKc?.ToPersisted(),
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeDhStatic ToPersistedDhStatic() => new ACVPCore.Algorithms.Persisted.KAS_FFC.SchemeDhStatic
			{
				Role = Role,
				KdfNoKc = KdfNoKc?.ToPersistedDhStatic(),
				KdfKc = KdfKc?.ToPersistedDhStatic()
			};
		}


		public class DhEphem : SchemeBase { }


		public class Mqv1 : SchemeBase { }


		public class DhHybrid1 : SchemeBase { }


		public class Mqv2 : SchemeBase { }


		public class DhHybridOneFlow : SchemeBase { }


		public class DhOneFlow : SchemeBase { }


		public class DhStatic : SchemeBase { }



		public class NoKdfNoKc
		{
			[JsonProperty("parameterSet")]
			public ParameterSets ParameterSets { get; set; }
		}


		public class KdfNoKc : NoKdfNoKc
		{
			[JsonProperty("kdfOption")]
			public KdfOptions KdfOption { get; set; }

			[JsonProperty("dkmNonceTypes", NullValueHandling = NullValueHandling.Ignore)]
			public List<string> DkmNonceTypes { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_FFC.KdfNoKc ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfNoKc
			{
				KdfOption = KdfOption?.ToPersisted(),
				ParameterSets = ParameterSets?.ToPersisted()
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.KdfNoKcDhStatic ToPersistedDhStatic() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfNoKcDhStatic
			{
				KdfOption = KdfOption?.ToPersisted(),
				ParameterSets = ParameterSets?.ToPersisted(),
				DkmNonceTypes = DkmNonceTypes
			};
		}


		public class KdfKc : KdfNoKc
		{
			[JsonProperty("kcOption")]
			public KcOptions KcOption { get; set; }

			public new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfKc ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfKc
			{
				KcOption = KcOption?.ToPersisted(),
				KdfOption = KdfOption?.ToPersisted(),
				ParameterSets = ParameterSets?.ToPersisted()
			};

			public new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfKcDhStatic ToPersistedDhStatic() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfKcDhStatic
			{
				KcOption = KcOption?.ToPersisted(),
				KdfOption = KdfOption?.ToPersisted(),
				ParameterSets = ParameterSets?.ToPersisted(),
				DkmNonceTypes = DkmNonceTypes
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

			public ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSets ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSets
			{
				FB = Fb?.ToPersistedParameterSetFB(),
				FC = Fc?.ToPersistedParameterSetFC()
			};
		}


		public class ParameterSet
		{
			[JsonProperty("hashAlg")]
			public List<string> HashAlg { get; set; } = new List<string>();

			[JsonProperty("macOption", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptions MacOption { get; set; } = new MacOptions();


			public ParameterSet(string optionPrefix, string parameterSetName, Dictionary<string, string> options)
			{
				//Possible lengths depends on the parameter set - FB has 224, otherwise they're the same
				int[] shaLens = parameterSetName == "FB" ? new int[] { 224, 256, 384, 512 } : new int[] { 256, 384, 512 };

				//Hash Algs
				foreach (int len in shaLens)
				{
					if (options.GetValue($"{optionPrefix}_{parameterSetName}_SHA{len}") == "True") HashAlg.Add($"SHA2-{len}");
				}

				//Mac options. This is a little goofy, they all work a little differently

				//AES-CCM - There isn't a single field to indicate this is selected, so build the object, then check to see if it is valid
				MacOptionsBase ccm = new MacOptionsBase();
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM128") == "True") ccm.KeyLen.Add(128);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM192") == "True") ccm.KeyLen.Add(192);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM256") == "True") ccm.KeyLen.Add(256);

				if (ccm.KeyLen.Count > 0)
				{
					//MacLen
					for (int i = 8; i < 17; i = i + 2)
					{
						if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM_TagLen{i}") == "True")
						{
							ccm.MacLen = 8 * i;
							break;
						}
					}

					//NonceLen
					for (int i = 7; i <= 13; i++)
					{
						if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM_NonceLen{i}") == "True")
						{
							ccm.NonceLen = 8 * i;
							break;
						}
					}

					//Finally add it
					MacOption.AesCcm = ccm;
				}

				//CMAC
				MacOptionsBase cmac = new MacOptionsBase();
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC128") == "True") cmac.KeyLen.Add(128);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC192") == "True") cmac.KeyLen.Add(192);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC256") == "True") cmac.KeyLen.Add(256);

				if (cmac.KeyLen.Count > 0)
				{
					//Some schemes/parameter sets have CMACTagLen, some have CMACAESTagLen and CMACTDESTagLen, even though TDES isn't legal. Look for CMACTagLen first
					if (options.ContainsKey($"{optionPrefix}_{parameterSetName}_CMACTagLen"))
					{
						cmac.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_CMACTagLen")) * 8;
					}
					else
					{
						//Since CMACTagLen didn't exist it must be CMACAESTagLen
						cmac.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_CMACAESTagLen")) * 8;
					}

					MacOption.Cmac = cmac;
				}

				//HMAC-SHA2-224 - only available for FB
				if (parameterSetName == "FB" && options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA224") == "True")
				{
					MacOptionsBase hmac224 = new MacOptionsBase();
					hmac224.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac224.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D224 = hmac224;
				}

				//HMAC-SHA2-256
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA256") == "True")
				{
					MacOptionsBase hmac256 = new MacOptionsBase();
					hmac256.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac256.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D256 = hmac256;
				}

				//HMAC-SHA2-384
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA384") == "True")
				{
					MacOptionsBase hmac384 = new MacOptionsBase();
					hmac384.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac384.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D384 = hmac384;
				}

				//HMAC-SHA2-512
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA512") == "True")
				{
					MacOptionsBase hmac512 = new MacOptionsBase();
					hmac512.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac512.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D512 = hmac512;
				}
			}

			public ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSetFB ToPersistedParameterSetFB() => new ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSetFB
			{
				HashAlg = HashAlg,
				MacOption = MacOption?.ToPersistedMacOptionsFB()
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSetFC ToPersistedParameterSetFC() => new ACVPCore.Algorithms.Persisted.KAS_FFC.ParameterSetFC
			{
				HashAlg = HashAlg,
				MacOption = MacOption?.ToPersistedMacOptionsFC()
			};
		}



		public class KdfOptions
		{
			[JsonProperty("concatenation", NullValueHandling = NullValueHandling.Ignore)]
			public string Concatenation { get; set; }

			[JsonProperty("asn1", NullValueHandling = NullValueHandling.Ignore)]
			public string Asn1 { get; set; }

			public KdfOptions(string optionPrefix, Dictionary<string, string> options)
			{
				if (options.GetValue($"{optionPrefix}_KDFConcat") == "True") Concatenation = "";    //Don't have the data that ACVP uses, so just make it non-null
				if (options.GetValue($"{optionPrefix}_KDFASN1") == "True") Asn1 = "";               //Don't have the data that ACVP uses, so just make it non-null
			}

			public ACVPCore.Algorithms.Persisted.KAS_FFC.KdfOptions ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KdfOptions
			{
				Concatenation = Concatenation,
				Asn1 = Asn1
			};
		}


		public class MacOptions
		{
			[JsonProperty(PropertyName = "AES-CCM", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase AesCcm { get; set; }

			[JsonProperty(PropertyName = "CMAC", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase Cmac { get; set; }

			[JsonProperty(PropertyName = "HMAC-SHA2-224", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase HmacSha2_D224 { get; set; }

			[JsonProperty(PropertyName = "HMAC-SHA2-256", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase HmacSha2_D256 { get; set; }

			[JsonProperty(PropertyName = "HMAC-SHA2-384", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase HmacSha2_D384 { get; set; }

			[JsonProperty(PropertyName = "HMAC-SHA2-512", NullValueHandling = NullValueHandling.Ignore)]
			public MacOptionsBase HmacSha2_D512 { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionsFB ToPersistedMacOptionsFB() => new ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionsFB
			{
				AesCcm = AesCcm?.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac?.ToPersistedMacOption(),
				HmacSha2_D224 = HmacSha2_D224?.ToPersistedMacOption(),
				HmacSha2_D256 = HmacSha2_D256?.ToPersistedMacOption(),
				HmacSha2_D384 = HmacSha2_D384?.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512?.ToPersistedMacOption()
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionsFC ToPersistedMacOptionsFC() => new ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionsFC
			{
				AesCcm = AesCcm?.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac?.ToPersistedMacOption(),
				HmacSha2_D256 = HmacSha2_D256?.ToPersistedMacOption(),
				HmacSha2_D384 = HmacSha2_D384?.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512?.ToPersistedMacOption()
			};

		}


		public class MacOptionsBase
		{
			[JsonProperty("keyLen")]
			public Domain KeyLen { get; set; } = new Domain();

			[JsonProperty("macLen")]
			public int MacLen { get; set; }

			[JsonProperty("nonceLen", NullValueHandling = NullValueHandling.Ignore)]
			public int? NonceLen { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_FFC.MacOption ToPersistedMacOption() => new ACVPCore.Algorithms.Persisted.KAS_FFC.MacOption
			{
				KeyLen = KeyLen.ToCoreDomain(),
				MacLen = MacLen
			};

			public ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionWithNonceLen ToPersistedMacOptionWithNonceLen() => new ACVPCore.Algorithms.Persisted.KAS_FFC.MacOptionWithNonceLen
			{
				KeyLen = KeyLen.ToCoreDomain(),
				MacLen = MacLen,
				NonceLen = NonceLen
			};
		}


		public class KcOptions
		{
			[JsonProperty("kcRole")]
			public List<string> KcRole { get; set; } = new List<string>();

			[JsonProperty("kcType")]
			public List<string> KcType { get; set; } = new List<string>();

			[JsonProperty("nonceType")]
			public List<string> NonceType { get; set; } = new List<string>();

			public KcOptions(string optionPrefix, Dictionary<string, string> options)
			{
				//Nonce type
				if (options.GetValue($"{optionPrefix}_KCNonce1") == "True") NonceType.Add("randomNonce");
				if (options.GetValue($"{optionPrefix}_KCNonce2") == "True") NonceType.Add("timestamp");
				if (options.GetValue($"{optionPrefix}_KCNonce3") == "True") NonceType.Add("sequence");
				if (options.GetValue($"{optionPrefix}_KCNonce4") == "True") NonceType.Add("timestampSequence");

				//Roles
				if (options.GetValue($"{optionPrefix}_Provider") == "True") KcRole.Add("provider");
				if (options.GetValue($"{optionPrefix}_Recipient") == "True") KcRole.Add("recipient");

				//Type
				if (options.GetValue($"{optionPrefix}_Provider") == "True") KcType.Add("unilateral");
				if (options.GetValue($"{optionPrefix}_Provider") == "True") KcType.Add("bilateral");
			}

			public ACVPCore.Algorithms.Persisted.KAS_FFC.KcOptions ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_FFC.KcOptions
			{
				KcRole = KcRole,
				KcType = KcType,
				NonceType = NonceType
			};
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KAS_FFC
		{
			Functions = Functions,
			Schemes = Schemes?.ToPersisted()
		};
	}
}
