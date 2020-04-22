using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class KASECCChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public KASECCChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//The ECC CDH component is a fairly small set of options. The KASECC_Component option is irrelevant, need to go to the next level down to see if they've actually done the CDH component (have to check 2 boxes in CAVS)
			algorithms.Add(ParseAlgorithmFromMode("ECC CDH", "KASECC_Comp_DLC_Prim", new List<string> {	"KASECC_Comp_DLC_Prim_P224",
																										"KASECC_Comp_DLC_Prim_P256",
																										"KASECC_Comp_DLC_Prim_P384",
																										"KASECC_Comp_DLC_Prim_P521",
																										"KASECC_Comp_DLC_Prim_K233",
																										"KASECC_Comp_DLC_Prim_K283",
																										"KASECC_Comp_DLC_Prim_K409",
																										"KASECC_Comp_DLC_Prim_K571",
																										"KASECC_Comp_DLC_Prim_B233",
																										"KASECC_Comp_DLC_Prim_B283",
																										"KASECC_Comp_DLC_Prim_B409",
																										"KASECC_Comp_DLC_Prim_B571",
																										"KASECC_KAS_Assurance_ElementDPG",
																										"KASECC_KAS_Assurance_ElementDPV",
																										"KASECC_KAS_Assurance_ElementKPG",
																										"KASECC_KAS_Assurance_ElementFV",
																										"KASECC_KAS_Assurance_ElementKR",
																										"KASECC_KAS_Assurance_ElementPV"
																										}));

			//Need to build the KAS-ECC and Component-KAS-ECC algorithms based on the ZZTestOnly option in each scheme

			//Start with the ones we absolutely know we need
			List<string> sharedOptions = new List<string> { "KASECC_PrerequisiteSet",
															"KAS_Prerequisite_ECDSA",
															"KAS_Prerequisite_SHA",
															"KAS_Prerequisite_RNG",
															"KAS_Prerequisite_DRBG",
															"KAS_Prerequisite_CCM",
															"KAS_Prerequisite_CMAC",
															"KAS_Prerequisite_HMAC",
															"KASECC_KAS_Assurances_Set",
															"KASECC_KAS_Assurance_ElementDPG",
															"KASECC_KAS_Assurance_ElementDPV",
															"KASECC_KAS_Assurance_ElementKPG",
															"KASECC_KAS_Assurance_ElementFV",
															"KASECC_KAS_Assurance_ElementPV",
															"KASECC_KAS_Assurance_ElementKR"
															};

			//Keep the schemes as a separate list for now, because we need to use them for 2 purposes - for their own value, and to determine if children are needed
			List<string> schemes = new List<string> {   "KASECC_FullUnified",
														"KASECC_FullMQV",
														"KASECC_EphemUnified",
														"KASECC_OnePassUnified",
														"KASECC_OnePassMQV",
														"KASECC_OnePassDH",
														"KASECC_StaticUnified"
													};

			List<string> normalOptions = new List<string>();
			List<string> componentOptions = new List<string>();

			//Get the keys for the children that are needed
			foreach (string scheme in schemes)
			{
				//Only care about schemes that are True
				if (Chunk.KeyValuePairs.Exists(x => x.Key == scheme && x.Value == "True"))
				{

					//Need to know if regular or component - get the appropriate keys for each and put in the appropriate list as needed - and yes, the parent option is ZZTestOnly, the children are ZZOnly
					if (Chunk.KeyValuePairs.Exists(x => x.Key == scheme + "_ZZTestOnly" && x.Value == "True"))
					{
						componentOptions.Add(scheme + "_ZZTestOnly");   //This adds the property we checked for the existence of above, as the below doesn't, and the registration builder uses it
						componentOptions.AddRange(Chunk.KeyValuePairs.Where(x => x.Key.StartsWith(scheme + "_ZZOnly")).Select(x => x.Key)); //Only want the ZZOnly options

						//Add the scheme option, which we know to be true
						componentOptions.Add(scheme);

						//Also need to add the initiator/responder options, which aren't under ZZTestOnly
						componentOptions.Add(scheme + "_Initiator");
						componentOptions.Add(scheme + "_Responder");
					}
					else
					{
						//Add the scheme option, which we know to be true
						normalOptions.Add(scheme);

						normalOptions.AddRange(Chunk.KeyValuePairs.Where(x => x.Key.StartsWith(scheme + "_") && !x.Key.Contains("ZZOnly")).Select(x => x.Key)); //Want everything but the ZZOnly options
					}

				}
			}

			//If had a proper ECC scheme, meaning this wasn't only a Component submission, add the KAS-ECC algo
			if (normalOptions.Count != 0)
			{
				//Create the algorithm
				InfAlgorithm algo = new InfAlgorithm { AlgorithmName = "KAS ECC" };

				//Add the base and schemes options
				normalOptions.AddRange(sharedOptions);
				//normalOptions.AddRange(schemes);

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, normalOptions));

				algorithms.Add(algo);
			}

			//If had a Component ECC scheme, meaning this wasn't only a regular submission, add the Component KAS-ECC algo
			if (componentOptions.Count != 0)
			{
				//Create the algorithm
				InfAlgorithm algo = new InfAlgorithm { AlgorithmName = "Component-KAS-ECC" };

				//Add the base and schemes options
				componentOptions.AddRange(sharedOptions);
				//componentOptions.AddRange(schemes);

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, componentOptions));

				algorithms.Add(algo);
			}

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}