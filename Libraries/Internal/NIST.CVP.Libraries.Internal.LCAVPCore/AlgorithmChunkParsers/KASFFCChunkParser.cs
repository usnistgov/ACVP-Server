using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class KASFFCChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public KASFFCChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Almost can just return everything, but the Component really screws things up. Need to check each scheme to see if ZZTestOnly is true, then only get the corresponding options if it is false - if true, then it is a Component validation of that scheme
			//Will need to build a list with some logic

			//Start with the ones we absolutely know we need
			List<string> sharedOptions = new List<string> {	"KASFFC_PrerequisiteSet",
															"KAS_Prerequisite_DSA",
															"KAS_Prerequisite_SHA",
															"KAS_Prerequisite_RNG",
															"KAS_Prerequisite_DRBG",
															"KAS_Prerequisite_CCM",
															"KAS_Prerequisite_CMAC",
															"KAS_Prerequisite_HMAC",
															"KASFFC_KAS_Assurances_Set",
															"KASFFC_KAS_Assurance_ElementDPG",
															"KASFFC_KAS_Assurance_ElementDPV",
															"KASFFC_KAS_Assurance_ElementKPG",
															"KASFFC_KAS_Assurance_ElementFV",
															"KASFFC_KAS_Assurance_ElementPV",
															"KASFFC_KAS_Assurance_ElementKR"
															};

			//Keep the schemes as a separate list for now, because we need to use them for 2 purposes - for their own value, and to determine if children are needed
			List<string> schemes = new List<string> {   "KASFFC_Hybrid1",
														"KASFFC_MQV2",
														"KASFFC_Ephem",
														"KASFFC_Hybrid1Flow",
														"KASFFC_MQV1",
														"KASFFC_OneFlow",
														"KASFFC_Static"
													};

			List<string> normalOptions = new List<string>();
			List<string> componentOptions = new List<string>();

			//Get the keys for the children that are needed
			foreach (string scheme in schemes)
			{
				//Only care about schemes that are True
				if (Chunk.KeyValuePairs.Exists(x => x.Key == scheme && x.Value == "True")){

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

						normalOptions.AddRange(Chunk.KeyValuePairs.Where(x => x.Key.StartsWith(scheme + "_") && !x.Key.Contains("ZZOnly")).Select(x => x.Key));	//Want everything but the ZZOnly options
					}

				}
			}

			//Create the return object
			List<InfAlgorithm> algos = new List<InfAlgorithm>();

			//If had a proper FFC scheme, meaning this wasn't only a Component submission, add the KAS-FFC algo
			if (normalOptions.Count != 0)
			{
				//Create the algorithm
				InfAlgorithm algo = new InfAlgorithm { AlgorithmName = "KAS FFC" };

				//Add the base and schemes options
				normalOptions.AddRange(sharedOptions);
				//normalOptions.AddRange(schemes);

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, normalOptions));

				algos.Add(algo);
			}

			//If had a Component FFC scheme, meaning this wasn't only a regular submission, add the Component KAS-FFC algo
			if (componentOptions.Count != 0)
			{
				//Create the algorithm
				InfAlgorithm algo = new InfAlgorithm { AlgorithmName = "Component-KAS-FFC" };

				//Add the base and schemes options
				componentOptions.AddRange(sharedOptions);
				//componentOptions.AddRange(schemes);

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, componentOptions));

				algos.Add(algo);
			}

			return algos;
		}
	}
}