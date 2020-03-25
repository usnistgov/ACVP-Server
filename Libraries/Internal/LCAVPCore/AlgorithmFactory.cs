using System.Collections.Generic;
using System.Linq;
using LCAVPCore.Registration.Algorithms;
using LCAVPCore.Registration.Algorithms.AES;
using LCAVPCore.Registration.Algorithms.Component;
using LCAVPCore.Registration.Algorithms.DRBG;
using LCAVPCore.Registration.Algorithms.DSA;
using LCAVPCore.Registration.Algorithms.ECDSA;
using LCAVPCore.Registration.Algorithms.HMAC;
using LCAVPCore.Registration.Algorithms.KAS;
using LCAVPCore.Registration.Algorithms.KDF;
using LCAVPCore.Registration.Algorithms.RSA;
using LCAVPCore.Registration.Algorithms.SHA_3;
using LCAVPCore.Registration.Algorithms.SHS;
using LCAVPCore.Registration.Algorithms.TDES;

namespace LCAVPCore
{
	public class AlgorithmFactory : IAlgorithmFactory
	{
		private readonly IDataProvider _dataProvider;

		public AlgorithmFactory(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}

		public List<IAlgorithm> GetAlgorithms(InfAlgorithm infAlgorithm)
		{
			List<IAlgorithm> output = new List<IAlgorithm>();

			//Just needs to add the right kind of object(s) built from the inf file data. Usually it is just 1 object, but some algorithms result in multiple due to the way the algorithm data models are built
			switch (infAlgorithm.AlgorithmName)
			{
				case "AES-CBC": output.Add(new AES_CBC(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CCM": output.Add(new AES_CCM(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CFB1": output.Add(new AES_CFB1(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CFB128": output.Add(new AES_CFB128(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CFB8": output.Add(new AES_CFB8(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CMAC": output.Add(new AES_CMAC(infAlgorithm.Options, _dataProvider)); break;
				case "AES-CTR": output.Add(new AES_CTR(infAlgorithm.Options, _dataProvider)); break;
				case "AES-ECB": output.Add(new AES_ECB(infAlgorithm.Options, _dataProvider)); break;
				case "AES-GCM": output.AddRange(Get_AES_GCM(infAlgorithm.Options, _dataProvider)); break;
				case "AES-GMAC": output.AddRange(Get_AES_GMAC(infAlgorithm.Options, _dataProvider)); break;
				case "AES-KW": output.Add(new AES_KW(infAlgorithm.Options, _dataProvider)); break;
				case "AES-KWP": output.Add(new AES_KWP(infAlgorithm.Options, _dataProvider)); break;
				case "AES-OFB": output.Add(new AES_OFB(infAlgorithm.Options, _dataProvider)); break;
				case "AES-XPN": output.AddRange(Get_AES_XPN(infAlgorithm.Options, _dataProvider)); break;
				case "AES-XTS": output.AddRange(Get_AES_XTS(infAlgorithm.Options, _dataProvider)); break;
				case "ANS 9.63": output.Add(new ANS_963(infAlgorithm.Options, _dataProvider)); break;
				case "Component-KAS-ECC": output.Add(new Component_KAS_ECC(infAlgorithm.Options, _dataProvider)); break;
				case "Component-KAS-FFC": output.Add(new Component_KAS_FFC(infAlgorithm.Options, _dataProvider)); break;
				//case "counterMode": output.Add(new Counter(infAlgorithm.Options)); break;
				case "CTR_DRBG": output.Add(new CTR(infAlgorithm.Options, _dataProvider)); break;
				//case "doublePipelineIterationMode": output.Add(new DoublePipeline(infAlgorithm.Options)); break;
				case "DSAKeyPair": output.Add(new DSAKeyPair(infAlgorithm.Options, _dataProvider)); break;
				case "DSAPQGGen": output.Add(new DSAPQGGen(infAlgorithm.Options, _dataProvider)); break;
				case "DSAPQGVer": output.Add(new DSAPQGVer(infAlgorithm.Options, _dataProvider)); break;
				case "DSASigGen": output.Add(new DSASigGen(infAlgorithm.Options, _dataProvider)); break;
				case "DSASigVer": output.Add(new DSASigVer(infAlgorithm.Options, _dataProvider)); break;
				case "ECC CDH": output.Add(new ECC_CDH(infAlgorithm.Options, _dataProvider)); break;
				case "ECDSAKeyPair": output.Add(new ECDSA_KeyPair(infAlgorithm.Options, _dataProvider)); break;
				case "ECDSAPKV": output.Add(new ECDSA_PKV(infAlgorithm.Options, _dataProvider)); break;
				case "ECDSASigGen": output.Add(new ECDSA_SigGen(infAlgorithm.Options, _dataProvider)); break;
				case "ECDSASigVer": output.Add(new ECDSA_SigVer(infAlgorithm.Options, _dataProvider)); break;
				case "ECDSA SigGen Component": output.Add(new ECDSASigGenComponent(infAlgorithm.Options, _dataProvider)); break;
				//case "feedbackMode": output.Add(new Feedback(infAlgorithm.Options)); break;
				case "Hash_Based DRBG": output.Add(new Hash(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC_Based DRBG": output.Add(new HMAC(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA-1": output.Add(new HMAC_SHA_1(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-224": output.Add(new HMAC_SHA2_224(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-256": output.Add(new HMAC_SHA2_256(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-384": output.Add(new HMAC_SHA2_384(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-512": output.Add(new HMAC_SHA2_512(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-512/224": output.Add(new HMAC_SHA2_512224(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA2-512/256": output.Add(new HMAC_SHA2_512256(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA3-224": output.Add(new HMAC_SHA3_224(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA3-256": output.Add(new HMAC_SHA3_256(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA3-384": output.Add(new HMAC_SHA3_384(infAlgorithm.Options, _dataProvider)); break;
				case "HMAC-SHA3-512": output.Add(new HMAC_SHA3_512(infAlgorithm.Options, _dataProvider)); break;
				case "IKEv1": output.Add(new IKEv1(infAlgorithm.Options, _dataProvider)); break;
				case "IKEv2": output.Add(new IKEv2(infAlgorithm.Options, _dataProvider)); break;
				case "KAS ECC": output.Add(new KAS_ECC(infAlgorithm.Options, _dataProvider)); break;
				case "KAS FFC": output.Add(new KAS_FFC(infAlgorithm.Options, _dataProvider)); break;
				case "KDF": output.Add(new KDF(infAlgorithm.Options, _dataProvider)); break;
				case "RSAKeyGen": output.AddRange(Get_RSAKeyGen(infAlgorithm.Options, _dataProvider)); break;
				case "RSAKeyGen186-2": output.Add(new RSAKeyGen186_2(infAlgorithm.Options, _dataProvider)); break;
				case "RSALegacySigVer": output.Add(new RSALegacySigVer(infAlgorithm.Options, _dataProvider)); break;
				case "RSASigGen": output.AddRange(Get_RSASigGen(infAlgorithm.Options, _dataProvider)); break;
				case "RSASigGen186-2": output.Add(new RSASigGen186_2(infAlgorithm.Options, _dataProvider)); break;
				case "RSASigVer": output.AddRange(Get_RSASigVer(infAlgorithm.Options, _dataProvider)); break;
				case "RSADP": output.Add(new RSADP(infAlgorithm.Options, _dataProvider)); break;
				case "RSASP1": output.Add(new RSASP1(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-1": output.Add(new SHA_1(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-224": output.Add(new SHA_224(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-256": output.Add(new SHA_256(infAlgorithm.Options, _dataProvider)); break;
				case "SHA3-224": output.Add(new SHA3_224(infAlgorithm.Options, _dataProvider)); break;
				case "SHA3-256": output.Add(new SHA3_256(infAlgorithm.Options, _dataProvider)); break;
				case "SHA3-384": output.Add(new SHA3_384(infAlgorithm.Options, _dataProvider)); break;
				case "SHA3-512": output.Add(new SHA3_512(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-384": output.Add(new SHA_384(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-512": output.Add(new SHA_512(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-512/224": output.Add(new SHA_512224(infAlgorithm.Options, _dataProvider)); break;
				case "SHA-512/256": output.Add(new SHA_512256(infAlgorithm.Options, _dataProvider)); break;
				case "SHAKE-128": output.Add(new SHAKE_128(infAlgorithm.Options, _dataProvider)); break;
				case "SHAKE-256": output.Add(new SHAKE_256(infAlgorithm.Options, _dataProvider)); break;
				case "SNMP": output.Add(new SNMP(infAlgorithm.Options, _dataProvider)); break;
				case "SRTP": output.Add(new SRTP(infAlgorithm.Options, _dataProvider)); break;
				case "SSH": output.Add(new SSH(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CBC": output.Add(new TDES_CBC(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CBC-I": output.Add(new TDES_CBCI(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB1": output.Add(new TDES_CFB1(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB64": output.Add(new TDES_CFB64(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB8": output.Add(new TDES_CFB8(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB-P1": output.Add(new TDES_CFBP1(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB-P64": output.Add(new TDES_CFBP64(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CFB-P8": output.Add(new TDES_CFBP8(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CMAC": output.Add(new TDES_CMAC(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-CTR": output.Add(new TDES_CTR(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-ECB": output.Add(new TDES_ECB(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-KW": output.Add(new TDES_KW(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-OFB": output.Add(new TDES_OFB(infAlgorithm.Options, _dataProvider)); break;
				case "TDES-OFB-I": output.Add(new TDES_OFBI(infAlgorithm.Options, _dataProvider)); break;
				case "TLS": output.Add(new TLS(infAlgorithm.Options, _dataProvider)); break;
				case "TPM": output.Add(new TPM(infAlgorithm.Options, _dataProvider)); break;
				default:
					break;
			}

			return output;
		}

		private List<IAlgorithm> Get_AES_XTS(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//Need to have separate registrations for 128/256
			List<IAlgorithm> output = new List<IAlgorithm>();

			if (options.GetValue("XTS_AES128") == "True")
			{
				output.Add(new AES_XTS(128, options, dataProvider));
			}

			if (options.GetValue("XTS_AES256") == "True")
			{
				output.Add(new AES_XTS(256, options, dataProvider));
			}

			return output;
		}

		private List<IAlgorithm> Get_RSAKeyGen(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//Can result in multiple registrations depending on what kind of e options are selected
			List<IAlgorithm> output = new List<IAlgorithm>();

			List<(string Mode, string Value)> eGenVariants = new List<(string Mode, string Value)>();

			if (options.GetValue("RSA2_Fixed_e") == "True")
			{
				eGenVariants.Add(("fixed", options.GetValue("RSA2_Fixed_e_Value")));
			}

			if (options.GetValue("RSA2_Random_e") == "True")
			{
				eGenVariants.Add(("random", null));
			}

			//Special case, where Probable Primes means neither e generation method is selected, which ACVP says is not possible. CAVS apparently assumes random
			if (options.GetValue("FIPS186_3KeyGen_ProbRP") == "True" && options.GetValue("RSA2_Fixed_e") == "False" && options.GetValue("RSA2_Random_e") == "False")
			{
				eGenVariants.Add(("random", null));
			}

			//Loop through the collection of e related data and generate as many algos as needed
			foreach (var (Mode, Value) in eGenVariants)
			{
				output.Add(new RSAKeyGen(Mode, Value, options, dataProvider));
			}

			return output;
		}

		private List<IAlgorithm> Get_RSASigGen(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//Because doing 186-2 SigGen will also make the conditions that identify 186-4 SigGen true, even if you did no 186-4 stuff, need another check to see whether or not we should add SigGen.
			//Easiest way to do this is just to go ahead and build the SigGen object, then see if it is garbage or not

			List<IAlgorithm> output = new List<IAlgorithm>();   //Using a list so I don't have to handle null results where this gets called

			RSASigGen sigGen = new RSASigGen(options, dataProvider);
			if (sigGen.Have186_4Capabilities)
			{
				output.Add(sigGen);
			}

			return output;
		}

		private List<IAlgorithm> Get_RSASigVer(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//Lots of weird variants of RSA SigVer that result in needing multiple registrations

			List<IAlgorithm> output = new List<IAlgorithm>();

			List<(string Mode, string Value)> eGenVariants = new List<(string Mode, string Value)>();

			//Grab all the variants from the 3 flavors, take the distinct at the end
			if (options.GetValue("FIPS186_3SigVer") == "True")
			{
				if (options.GetValue("FIPS186_3SigVer_Random_e_Value") == "True")
				{
					//Random e
					eGenVariants.Add(("random", null));
				}

				if (options.GetValue("FIPS186_3SigVer_Fixed_e_Value") == "True")
				{
					//Fixed e, may have 2 values, Min and Max in the inf (but not on the screen). But maybe they didn't specify either...
					bool haveFixedEValue = false;

					//Add Min if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Min")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Min")));
						haveFixedEValue = true;
					}

					//Add Max if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Max")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Max")));
						haveFixedEValue = true;
					}

					//If they didn't specify one, still need to say that it was fixed, even if we don't know the value
					if (!haveFixedEValue)
					{
						eGenVariants.Add(("fixed", null));
					}
				}
			}

			if (options.GetValue("FIPS186_3SigVerPKCS15") == "True")
			{
				if (options.GetValue("FIPS186_3SigVerPKCS15_Random_e_Value") == "True")
				{
					//Random e
					eGenVariants.Add(("random", null));
				}

				if (options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value") == "True")
				{
					//Fixed e, may have 2 values, Min and Max in the inf (but not on the screen). But maybe they didn't specify either...
					bool haveFixedEValue = false;

					//Add Min if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Min")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Min")));
						haveFixedEValue = true;
					}

					//Add Max if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Max")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Max")));
						haveFixedEValue = true;
					}

					//If they didn't specify one, still need to say that it was fixed, even if we don't know the value
					if (!haveFixedEValue)
					{
						eGenVariants.Add(("fixed", null));
					}
				}
			}

			if (options.GetValue("FIPS186_3SigVerPKCSPSS") == "True")
			{
				if (options.GetValue("FIPS186_3SigVerPKCSPSS_Random_e_Value") == "True")
				{
					//Random e
					eGenVariants.Add(("random", null));
				}

				if (options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value") == "True")
				{
					//Fixed e, may have 2 values, Min and Max in the inf (but not on the screen). But maybe they didn't specify either...
					bool haveFixedEValue = false;

					//Add Min if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Min")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Min")));
						haveFixedEValue = true;
					}

					//Add Max if it is populated
					if (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Max")) != null)
					{
						eGenVariants.Add(("fixed", options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Max")));
						haveFixedEValue = true;
					}

					//If they didn't specify one, still need to say that it was fixed, even if we don't know the value
					if (!haveFixedEValue)
					{
						eGenVariants.Add(("fixed", null));
					}
				}
			}

			//Just in case they didn't provide anything about e, still need something here to actually generate SigVer
			if (eGenVariants.Count == 0)
			{
				eGenVariants.Add((null, null));
			}

			//Loop through the collection of e related data and generate as many algos as needed
			foreach (var (Mode, Value) in eGenVariants.Distinct())
			{
				output.Add(new RSASigVer(Mode, Value, options, dataProvider));
			}

			return output;
		}

		private List<IAlgorithm> Get_AES_GCM(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//JSON only supports a single value for the IV Gen stuff, so may need to do the algo multiple times. Also need to cover when they say internal but don't specify which mode
			List<IAlgorithm> output = new List<IAlgorithm>();

			List<(string IVGenLocation, string IVGenMode)> ivGenData = new List<(string IVGenLocation, string IVGenMode)>();

			if (options.GetValue("InternalIV") == "True")
			{
				bool haveMode = false;

				if ((options.GetValue("Affirm8_2_1") == "True"))
				{
					ivGenData.Add(("internal", "8.2.1"));
					haveMode = true;
				}

				if ((options.GetValue("Affirm8_2_2") == "True"))
				{
					ivGenData.Add(("internal", "8.2.2"));
					haveMode = true;
				}

				if (!haveMode)
				{
					ivGenData.Add(("internal", null));
				}
			}
			else
			{
				ivGenData.Add(("external", null));
			}

			foreach (var (IVGenLocation, IVGenMode) in ivGenData)
			{
				output.Add(new AES_GCM(IVGenLocation, IVGenMode, options, dataProvider));
			}

			return output;
		}

		private List<IAlgorithm> Get_AES_GMAC(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//JSON only supports a single value for the IV Gen stuff, so may need to do the algo multiple times. Also need to cover when they say internal but don't specify which mode
			List<IAlgorithm> output = new List<IAlgorithm>();

			List<(string IVGenLocation, string IVGenMode)> ivGenData = new List<(string IVGenLocation, string IVGenMode)>();

			if (options.GetValue("InternalIV") == "True")
			{
				bool haveMode = false;

				if ((options.GetValue("Affirm8_2_1") == "True"))
				{
					ivGenData.Add(("internal", "8.2.1"));
					haveMode = true;
				}

				if ((options.GetValue("Affirm8_2_2") == "True"))
				{
					ivGenData.Add(("internal", "8.2.2"));
					haveMode = true;
				}

				if (!haveMode)
				{
					ivGenData.Add(("internal", null));
				}
			}
			else
			{
				ivGenData.Add(("external", null));
			}

			foreach (var (IVGenLocation, IVGenMode) in ivGenData)
			{
				output.Add(new AES_GMAC(IVGenLocation, IVGenMode, options, dataProvider));
			}

			return output;
		}

		private List<IAlgorithm> Get_AES_XPN(Dictionary<string, string> options, IDataProvider dataProvider)
		{
			//JSON only supports a single value for the IV Gen stuff, so may need to do the algo multiple times. Also need to cover when they say internal but don't specify which mode
			List<IAlgorithm> output = new List<IAlgorithm>();

			List<(string IVGenLocation, string IVGenMode)> ivGenData = new List<(string IVGenLocation, string IVGenMode)>();

			if (options.GetValue("InternalIV") == "True")
			{
				bool haveMode = false;

				if ((options.GetValue("Affirm8_2_1") == "True"))
				{
					ivGenData.Add(("internal", "8.2.1"));
					haveMode = true;
				}

				if ((options.GetValue("Affirm8_2_2") == "True"))
				{
					ivGenData.Add(("internal", "8.2.2"));
					haveMode = true;
				}

				if (!haveMode)
				{
					ivGenData.Add(("internal", null));
				}
			}
			else
			{
				ivGenData.Add(("external", null));
			}

			foreach (var (IVGenLocation, IVGenMode) in ivGenData)
			{
				output.Add(new AES_XPN(IVGenLocation, IVGenMode, options, dataProvider));
			}

			return output;
		}
	}
}
