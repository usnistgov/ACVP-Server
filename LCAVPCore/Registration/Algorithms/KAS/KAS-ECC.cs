using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.KAS
{
	public class KAS_ECC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Functions { get; private set; }

		[JsonProperty("scheme")]
		public SchemeCollection Schemes { get; private set; } = new SchemeCollection();

		public KAS_ECC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KAS-ECC")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("ECDSA", options.GetValue("KAS_Prerequisite_ECDSA")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KAS_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("KAS_Prerequisite_DRBG")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KAS_Prerequisite_CCM")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KAS_Prerequisite_CMAC")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KAS_Prerequisite_HMAC")));

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
			if (options.GetValue("KASECC_FullUnified") == "True")
			{
				Schemes.FullUnified = new FullUnified();

				//Roles
				if (options.GetValue("KASECC_FullUnified_Initiator") == "True") Schemes.FullUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_FullUnified_Responder") == "True") Schemes.FullUnified.Role.Add("responder");

				if (options.GetValue("KASECC_FullUnified_KeyConfirm") == "True")
				{
					Schemes.FullUnified.KdfKc = new KdfKc();
					Schemes.FullUnified.KdfKc.KdfOption = new KdfOptions("KASECC_FullUnified", options);
					Schemes.FullUnified.KdfKc.KcOption = new KcOptions("KASECC_FullUnified", options);
					Schemes.FullUnified.KdfKc.ParameterSets = new ParameterSets("KASECC_FullUnified_KC", options);
				}
				else
				{
					Schemes.FullUnified.KdfNoKc = new KdfNoKc();
					Schemes.FullUnified.KdfNoKc.KdfOption = new KdfOptions("KASECC_FullUnified", options);
					Schemes.FullUnified.KdfNoKc.ParameterSets = new ParameterSets("KASECC_FullUnified_NOKC", options);
				}
			}


			//FullMQV
			if (options.GetValue("KASECC_FullMQV") == "True")
			{
				Schemes.FullMQV = new FullMQV();

				//Roles
				if (options.GetValue("KASECC_FullMQV_Initiator") == "True") Schemes.FullMQV.Role.Add("initiator");
				if (options.GetValue("KASECC_FullMQV_Responder") == "True") Schemes.FullMQV.Role.Add("responder");

				if (options.GetValue("KASECC_FullMQV_KeyConfirm") == "True")
				{
					Schemes.FullMQV.KdfKc = new KdfKc();
					Schemes.FullMQV.KdfKc.KdfOption = new KdfOptions("KASECC_FullMQV", options);
					Schemes.FullMQV.KdfKc.KcOption = new KcOptions("KASECC_FullMQV", options);
					Schemes.FullMQV.KdfKc.ParameterSets = new ParameterSets("KASECC_FullMQV_KC", options);
				}
				else
				{
					Schemes.FullMQV.KdfNoKc = new KdfNoKc();
					Schemes.FullMQV.KdfNoKc.KdfOption = new KdfOptions("KASECC_FullMQV", options);
					Schemes.FullMQV.KdfNoKc.ParameterSets = new ParameterSets("KASECC_FullMQV_NOKC", options);
				}
			}


			//EphemeralUnified
			if (options.GetValue("KASECC_EphemUnified") == "True")
			{
				Schemes.EphemeralUnified = new EphemeralUnified();

				//Roles
				if (options.GetValue("KASECC_EphemUnified_Initiator") == "True") Schemes.EphemeralUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_EphemUnified_Responder") == "True") Schemes.EphemeralUnified.Role.Add("responder");

				//Ephem can't have KC, so this will have KdfNoKc populated
				Schemes.EphemeralUnified.KdfNoKc = new KdfNoKc();

				Schemes.EphemeralUnified.KdfNoKc.KdfOption = new KdfOptions("KASECC_EphemUnified", options);
				Schemes.EphemeralUnified.KdfNoKc.ParameterSets = new ParameterSets("KASECC_EphemUnified_NOKC", options);
			}


			//OnePassUnified
			if (options.GetValue("KASECC_OnePassUnified") == "True")
			{
				Schemes.OnePassUnified = new OnePassUnified();

				//Roles
				if (options.GetValue("KASECC_OnePassUnified_Initiator") == "True") Schemes.OnePassUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassUnified_Responder") == "True") Schemes.OnePassUnified.Role.Add("responder");

				if (options.GetValue("KASECC_OnePassUnified_KeyConfirm") == "True")
				{
					Schemes.OnePassUnified.KdfKc = new KdfKc();
					Schemes.OnePassUnified.KdfKc.KdfOption = new KdfOptions("KASECC_OnePassUnified", options);
					Schemes.OnePassUnified.KdfKc.KcOption = new KcOptions("KASECC_OnePassUnified", options);
					Schemes.OnePassUnified.KdfKc.ParameterSets = new ParameterSets("KASECC_OnePassUnified_KC", options);
				}
				else
				{
					Schemes.OnePassUnified.KdfNoKc = new KdfNoKc();
					Schemes.OnePassUnified.KdfNoKc.KdfOption = new KdfOptions("KASECC_OnePassUnified", options);
					Schemes.OnePassUnified.KdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassUnified_NOKC", options);
				}
			}


			//OnePassMQV
			if (options.GetValue("KASECC_OnePassMQV") == "True")
			{
				Schemes.OnePassMQV = new OnePassMQV();

				//Roles
				if (options.GetValue("KASECC_OnePassMQV_Initiator") == "True") Schemes.OnePassMQV.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassMQV_Responder") == "True") Schemes.OnePassMQV.Role.Add("responder");

				if (options.GetValue("KASECC_OnePassMQV_KeyConfirm") == "True")
				{
					Schemes.OnePassMQV.KdfKc = new KdfKc();
					Schemes.OnePassMQV.KdfKc.KdfOption = new KdfOptions("KASECC_OnePassMQV", options);
					Schemes.OnePassMQV.KdfKc.KcOption = new KcOptions("KASECC_OnePassMQV", options);
					Schemes.OnePassMQV.KdfKc.ParameterSets = new ParameterSets("KASECC_OnePassMQV_KC", options);
				}
				else
				{
					Schemes.OnePassMQV.KdfNoKc = new KdfNoKc();
					Schemes.OnePassMQV.KdfNoKc.KdfOption = new KdfOptions("KASECC_OnePassMQV", options);
					Schemes.OnePassMQV.KdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassMQV_NOKC", options);
				}
			}


			//OnePassDH
			if (options.GetValue("KASECC_OnePassDH") == "True")
			{
				Schemes.OnePassDH = new OnePassDH();

				//Roles
				if (options.GetValue("KASECC_OnePassDH_Initiator") == "True") Schemes.OnePassDH.Role.Add("initiator");
				if (options.GetValue("KASECC_OnePassDH_Responder") == "True") Schemes.OnePassDH.Role.Add("responder");

				if (options.GetValue("KASECC_OnePassDH_KeyConfirm") == "True")
				{
					Schemes.OnePassDH.KdfKc = new KdfKc();
					Schemes.OnePassDH.KdfKc.KdfOption = new KdfOptions("KASECC_OnePassDH", options);
					Schemes.OnePassDH.KdfKc.KcOption = new KcOptions("KASECC_OnePassDH", options);      //This will be kind of wrong, as OnePassDH implies certain rules for the Roles and Type. Those will be set properly in subsequent code, but the nonce stuff here is still good
					Schemes.OnePassDH.KdfKc.ParameterSets = new ParameterSets("KASECC_OnePassDH_KC", options);

					//Special OnePassDH KcOption rules
					//KC roles depend on initiator/responder
					if (options.GetValue("KASECC_OnePassDH_Initiator") == "True") Schemes.OnePassDH.KdfKc.KcOption.KcRole.Add("recipient");
					if (options.GetValue("KASECC_OnePassDH_Responder") == "True") Schemes.OnePassDH.KdfKc.KcOption.KcRole.Add("provider");

					//Can't do bilateral, so must be unilateral
					Schemes.OnePassDH.KdfKc.KcOption.KcType.Add("unilateral");
				}
				else
				{
					Schemes.OnePassDH.KdfNoKc = new KdfNoKc();
					Schemes.OnePassDH.KdfNoKc.KdfOption = new KdfOptions("KASECC_OnePassDH", options);
					Schemes.OnePassDH.KdfNoKc.ParameterSets = new ParameterSets("KASECC_OnePassDH_NOKC", options);
				}
			}


			//StaticUnified
			if (options.GetValue("KASECC_StaticUnified") == "True")
			{
				Schemes.StaticUnified = new StaticUnified();

				//Roles
				if (options.GetValue("KASECC_StaticUnified_Initiator") == "True") Schemes.StaticUnified.Role.Add("initiator");
				if (options.GetValue("KASECC_StaticUnified_Responder") == "True") Schemes.StaticUnified.Role.Add("responder");

				if (options.GetValue("KASECC_StaticUnified_KeyConfirm") == "True")
				{
					Schemes.StaticUnified.KdfKc = new KdfKc();
					Schemes.StaticUnified.KdfKc.KdfOption = new KdfOptions("KASECC_StaticUnified", options);
					Schemes.StaticUnified.KdfKc.KcOption = new KcOptions("KASECC_StaticUnified", options);
					Schemes.StaticUnified.KdfKc.ParameterSets = new ParameterSets("KASECC_StaticUnified_KC", options);
					Schemes.StaticUnified.KdfKc.DkmNonceTypes = BuildDkmNonceTypes(options);
				}
				else
				{
					Schemes.StaticUnified.KdfNoKc = new KdfNoKc();
					Schemes.StaticUnified.KdfNoKc.KdfOption = new KdfOptions("KASECC_StaticUnified", options);
					Schemes.StaticUnified.KdfNoKc.ParameterSets = new ParameterSets("KASECC_StaticUnified_NOKC", options);
					Schemes.StaticUnified.KdfNoKc.DkmNonceTypes = BuildDkmNonceTypes(options);
				}
			}
		}

		private List<string> BuildDkmNonceTypes(Dictionary<string, string> options)
		{
			List<string> result = new List<string>();
			if (options.GetValue("KASECC_StaticUnified_StaticNonce1") == "True") result.Add("randomNonce");
			if (options.GetValue("KASECC_StaticUnified_StaticNonce2") == "True") result.Add("timestamp");
			if (options.GetValue("KASECC_StaticUnified_StaticNonce3") == "True") result.Add("sequence");
			if (options.GetValue("KASECC_StaticUnified_StaticNonce4") == "True") result.Add("timestampSequence");
			return result.Count > 0 ? result : null;
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.SchemeCollection ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.SchemeCollection
			{
				EphemeralUnified = EphemeralUnified.ToPersisted(),
				FullUnified = FullUnified.ToPersisted(),
				FullMQV = FullMQV.ToPersisted(),
				OnePassUnified = OnePassUnified.ToPersisted(),
				OnePassMQV = OnePassMQV.ToPersisted(),
				OnePassDH = OnePassDH.ToPersisted(),
				StaticUnified = StaticUnified.ToPersistedStaticUnified()
			};
		}

		public abstract class SchemeBase
		{
			[JsonProperty("kasRole")]
			public List<string> Role { get; set; } = new List<string>();

			[JsonProperty("kdfNoKc", NullValueHandling = NullValueHandling.Ignore)]
			public KdfNoKc KdfNoKc { get; set; }

			[JsonProperty("kdfKc", NullValueHandling = NullValueHandling.Ignore)]
			public KdfKc KdfKc { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_ECC.Scheme ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.Scheme
			{
				Role = Role,
				KdfNoKc = KdfNoKc.ToPersisted(),
				KdfKc = KdfKc.ToPersisted()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.SchemeStaticUnified ToPersistedStaticUnified() => new ACVPCore.Algorithms.Persisted.KAS_ECC.SchemeStaticUnified
			{
				Role = Role,
				KdfNoKc = KdfNoKc.ToPersistedStaticUnified(),
				KdfKc = KdfKc.ToPersistedStaticUnified()
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
		}


		public class KdfNoKc : NoKdfNoKc
		{
			[JsonProperty("kdfOption")]
			public KdfOptions KdfOption { get; set; }

			[JsonProperty("dkmNonceTypes", NullValueHandling = NullValueHandling.Ignore)]
			public List<string> DkmNonceTypes { get; set; }

			public ACVPCore.Algorithms.Persisted.KAS_ECC.KdfNoKc ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfNoKc
			{
				KdfOption = KdfOption.ToPersisted(),
				ParameterSets = ParameterSets.ToPersisted()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.KdfNoKcStaticUnified ToPersistedStaticUnified() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfNoKcStaticUnified
			{
				KdfOption = KdfOption.ToPersisted(),
				ParameterSets = ParameterSets.ToPersisted(),
				DkmNonceTypes = DkmNonceTypes
			};
		}


		public class KdfKc : KdfNoKc
		{
			[JsonProperty("kcOption")]
			public KcOptions KcOption { get; set; }

			public new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfKc ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfKc
			{
				KcOption = KcOption.ToPersisted(),
				KdfOption = KdfOption.ToPersisted(),
				ParameterSets = ParameterSets.ToPersisted()
			};

			public new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfKcStaticUnified ToPersistedStaticUnified() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfKcStaticUnified
			{
				KcOption = KcOption.ToPersisted(),
				KdfOption = KdfOption.ToPersisted(),
				ParameterSets = ParameterSets.ToPersisted(),
				DkmNonceTypes = DkmNonceTypes
			};
		}




		public class ParameterSets
		{
			[JsonProperty("eb", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSetBase EB { get; set; }

			[JsonProperty("ec", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSetBase EC { get; set; }

			[JsonProperty("ed", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSetBase ED { get; set; }

			[JsonProperty("ee", NullValueHandling = NullValueHandling.Ignore)]
			public ParameterSetBase EE { get; set; }

			public ParameterSets(string optionPrefix, Dictionary<string, string> options)
			{
				if (options.GetValue($"{optionPrefix}_EB") == "True") EB = new ParameterSetBase(optionPrefix, "EB", options);
				if (options.GetValue($"{optionPrefix}_EC") == "True") EC = new ParameterSetBase(optionPrefix, "EC", options);
				if (options.GetValue($"{optionPrefix}_ED") == "True") ED = new ParameterSetBase(optionPrefix, "ED", options);
				if (options.GetValue($"{optionPrefix}_EE") == "True") EE = new ParameterSetBase(optionPrefix, "EE", options);
			}

			public ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSets ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSets
			{
				EB = EB.ToPersistedParameterSetEB(),
				EC = EC.ToPersistedParameterSetEC(),
				ED = ED.ToPersistedParameterSetED(),
				EE = EE.ToPersistedParameterSetEE()
			};

		}


		public class ParameterSetBase
		{
			[JsonProperty("curve")]
			public string Curve { get; set; }

			[JsonProperty("hashAlg")]
			public List<string> HashAlg { get; set; } = new List<string>();

			[JsonProperty("macOption")]
			public MacOptions MacOption { get; set; } = new MacOptions();

			public ParameterSetBase(string optionPrefix, string parameterSetName, Dictionary<string, string> options)
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

				//Mac options. This is a little goofy, they all work a little differently

				//AES-CCM - There isn't a single field to indicate this is selected, so build the object, then check to see if it is valid
				MacOptionsBase ccm = new MacOptionsBase();
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM128") == "True") ccm.KeyLen.Add(128);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM192") == "True") ccm.KeyLen.Add(192);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM256") == "True") ccm.KeyLen.Add(256);

				for (int i = 7; i <= 13; i++)
				{
					if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM_NonceLen{i}") == "True")
					{
						ccm.NonceLen = 8 * i;
						break;
					}
				}

				for (int i = 8; i <= 16; i = i + 2)
				{
					if (options.GetValue($"{optionPrefix}_{parameterSetName}_CCM_TagLen{i}") == "True")
					{
						ccm.MacLen = 8 * i;
						break;
					}
				}

				if (ccm.KeyLen.Count > 0) MacOption.AesCcm = ccm;

				//CMAC
				MacOptionsBase cmac = new MacOptionsBase();
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC128") == "True") cmac.KeyLen.Add(128);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC192") == "True") cmac.KeyLen.Add(192);
				if (options.GetValue($"{optionPrefix}_{parameterSetName}_CMAC256") == "True") cmac.KeyLen.Add(256);

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

				if (cmac.KeyLen.Count > 0) MacOption.Cmac = cmac;


				//HMAC-SHA2-224
				if (parameterSetName == "EB" && options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA224") == "True")
				{
					MacOptionsBase hmac224 = new MacOptionsBase();
					hmac224.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac224.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D224 = hmac224;
				}

				//HMAC-SHA2-256
				if ((parameterSetName == "EB" || parameterSetName == "EC") && options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA256") == "True")
				{
					MacOptionsBase hmac256 = new MacOptionsBase();
					hmac256.KeyLen.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACKeySize")) * 8);
					hmac256.MacLen = ParsingHelper.ParseValueToInteger(options.GetValue($"{optionPrefix}_{parameterSetName}_HMACTagLen")) * 8;
					MacOption.HmacSha2_D256 = hmac256;
				}

				//HMAC-SHA2-384
				if ((parameterSetName == "EB" || parameterSetName == "EC" || parameterSetName == "ED") && options.GetValue($"{optionPrefix}_{parameterSetName}_HMACSHA384") == "True")
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEB ToPersistedParameterSetEB() => new ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEB
			{
				Curve = Curve,
				HashAlg = HashAlg,
				MacOption = MacOption.ToPersistedMacOptionsEB()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEC ToPersistedParameterSetEC() => new ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEC
			{
				Curve = Curve,
				HashAlg = HashAlg,
				MacOption = MacOption.ToPersistedMacOptionsEC()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetED ToPersistedParameterSetED() => new ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetED
			{
				Curve = Curve,
				HashAlg = HashAlg,
				MacOption = MacOption.ToPersistedMacOptionsED()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEE ToPersistedParameterSetEE() => new ACVPCore.Algorithms.Persisted.KAS_ECC.ParameterSetEE
			{
				Curve = Curve,
				HashAlg = HashAlg,
				MacOption = MacOption.ToPersistedMacOptionsEE()
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.KdfOptions ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KdfOptions
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEB ToPersistedMacOptionsEB() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEB
			{
				AesCcm = AesCcm.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac.ToPersistedMacOption(),
				HmacSha2_D224 = HmacSha2_D224.ToPersistedMacOption(),
				HmacSha2_D256 = HmacSha2_D256.ToPersistedMacOption(),
				HmacSha2_D384 = HmacSha2_D384.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512.ToPersistedMacOption()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEC ToPersistedMacOptionsEC() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEC
			{
				AesCcm = AesCcm.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac.ToPersistedMacOption(),
				HmacSha2_D256 = HmacSha2_D256.ToPersistedMacOption(),
				HmacSha2_D384 = HmacSha2_D384.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512.ToPersistedMacOption()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsED ToPersistedMacOptionsED() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsED
			{
				AesCcm = AesCcm.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac.ToPersistedMacOption(),
				HmacSha2_D384 = HmacSha2_D384.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512.ToPersistedMacOption()
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEE ToPersistedMacOptionsEE() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionsEE
			{
				AesCcm = AesCcm.ToPersistedMacOptionWithNonceLen(),
				Cmac = Cmac.ToPersistedMacOption(),
				HmacSha2_D512 = HmacSha2_D512.ToPersistedMacOption()
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOption ToPersistedMacOption() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOption
			{
				KeyLen = KeyLen.ToCoreDomain(),
				MacLen = MacLen
			};

			public ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionWithNonceLen ToPersistedMacOptionWithNonceLen() => new ACVPCore.Algorithms.Persisted.KAS_ECC.MacOptionWithNonceLen
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

			public ACVPCore.Algorithms.Persisted.KAS_ECC.KcOptions ToPersisted() => new ACVPCore.Algorithms.Persisted.KAS_ECC.KcOptions
			{
				KcRole = KcRole,
				KcType = KcType,
				NonceType = NonceType
			};

		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KAS_ECC
		{
			Functions = Functions,
			Schemes = Schemes.ToPersisted()
		};

	}

}
