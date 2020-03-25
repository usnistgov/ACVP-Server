using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class KDF_800_108ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public KDF_800_108ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//KDF is a single algorithm, so just need to convert the format
			return new List<InfAlgorithm> { new InfAlgorithm("KDF", Chunk.KeyValuePairs) };

			////Read through the chunk, build up algorithms
			//List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//algorithms.Add(ParseAlgorithmFromMode("counterMode", "KDF108_CTRMode", new List<string> {	"KDF108_Prerequisite_KAS",
			//																							"KDF108_Prerequisite_DRBG",
			//																							"KDF108_Prerequisite_CMAC",
			//																							"KDF108_Prerequisite_HMAC",
			//																							"KDF108_CTRModeKIGen56A",
			//																							"KDF108_CTRModeKIGen56B",
			//																							"KDF108_CTRModeKIGen90",
			//																							"KDF108_CTRModeKIGenRNG186",
			//																							"KDF108_CTRModeKIGenRNG931",
			//																							"KDF108_CTRModeKIGenRNG962",
			//																							"KDF108_CTRModeKIGen_NA",
			//																							"KDF108_CTRModeLlenFULLMin",
			//																							"KDF108_CTRModeLlenFULLMax",
			//																							"KDF108_CTRModeLlenPARTMin",
			//																							"KDF108_CTRModeLlenPARTMax",
			//																							"KDF108_CTRModePRFCMACAES128",
			//																							"KDF108_CTRModePRFCMACAES192",
			//																							"KDF108_CTRModePRFCMACAES256",
			//																							"KDF108_CTRModePRFCMACTDES2",
			//																							"KDF108_CTRModePRFCMACTDES3",
			//																							"KDF108_CTRModePRFHMACSHA1",
			//																							"KDF108_CTRModePRFHMACSHA224",
			//																							"KDF108_CTRModePRFHMACSHA256",
			//																							"KDF108_CTRModePRFHMACSHA384",
			//																							"KDF108_CTRModePRFHMACSHA512",
			//																							"KDF108_CTRModePRFCounterBefore",
			//																							"KDF108_CTRModePRFCounterAfter",
			//																							"KDF108_CTRModePRFCounterMiddle",
			//																							"KDF108_CTRModeRlen8",
			//																							"KDF108_CTRModeRlen16",
			//																							"KDF108_CTRModeRlen24",
			//																							"KDF108_CTRModeRlen32"
			//																						}));




			//algorithms.Add(ParseAlgorithmFromMode("feedbackMode", "KDF108_FeedbackMode", new List<string> {	"KDF108_Prerequisite_KAS",
			//																								"KDF108_Prerequisite_DRBG",
			//																								"KDF108_Prerequisite_CMAC",
			//																								"KDF108_Prerequisite_HMAC",
			//																								"KDF108_FDBKModeKIGen56A",
			//																								"KDF108_FDBKModeKIGen56B",
			//																								"KDF108_FDBKModeKIGen90",
			//																								"KDF108_FDBKModeKIGenRNG186",
			//																								"KDF108_FDBKModeKIGenRNG931",
			//																								"KDF108_FDBKModeKIGenRNG962",
			//																								"KDF108_FDBKModeKIGen_NA",
			//																								"KDF108_FDBKModeLlenFULLMin",
			//																								"KDF108_FDBKModeLlenFULLMax",
			//																								"KDF108_FDBKModeLlenPARTMin",
			//																								"KDF108_FDBKModeLlenPARTMax",
			//																								"KDF108_FDBKModePRFCMACAES128",
			//																								"KDF108_FDBKModePRFCMACAES192",
			//																								"KDF108_FDBKModePRFCMACAES256",
			//																								"KDF108_FDBKModePRFCMACTDES2",
			//																								"KDF108_FDBKModePRFCMACTDES3",
			//																								"KDF108_FDBKModePRFHMACSHA1",
			//																								"KDF108_FDBKModePRFHMACSHA224",
			//																								"KDF108_FDBKModePRFHMACSHA256",
			//																								"KDF108_FDBKModePRFHMACSHA384",
			//																								"KDF108_FDBKModePRFHMACSHA512",
			//																								"KDF108_FDBKModeZeroLenIVNOTSupported",
			//																								"KDF108_FDBKModeCtrInIterVar",
			//																								"KDF108_FDBKModeCtrB4IterVar",
			//																								"KDF108_FDBKModeCtrAftIterVar",
			//																								"KDF108_FDBKModeCtrAftFixedVar",
			//																								"KDF108_FDBKModeRlen8",
			//																								"KDF108_FDBKModeRlen16",
			//																								"KDF108_FDBKModeRlen24",
			//																								"KDF108_FDBKModeRlen32"
			//																								}));


			//algorithms.Add(ParseAlgorithmFromMode("doublePipelineIterationMode", "KDF108_PipelineMode", new List<string> {  "KDF108_Prerequisite_KAS",
			//																												"KDF108_Prerequisite_DRBG",
			//																												"KDF108_Prerequisite_CMAC",
			//																												"KDF108_Prerequisite_HMAC",
			//																												"KDF108_PIPEModeKIGen56A",
			//																												"KDF108_PIPEModeKIGen56B",
			//																												"KDF108_PIPEModeKIGen90",
			//																												"KDF108_PIPEModeKIGenRNG186",
			//																												"KDF108_PIPEModeKIGenRNG931",
			//																												"KDF108_PIPEModeKIGenRNG962",
			//																												"KDF108_PIPEModeKIGen_NA",
			//																												"KDF108_PIPEModeLlenFULLMin",
			//																												"KDF108_PIPEModeLlenFULLMax",
			//																												"KDF108_PIPEModeLlenPARTMin",
			//																												"KDF108_PIPEModeLlenPARTMax",
			//																												"KDF108_PIPEModePRFCMACAES128",
			//																												"KDF108_PIPEModePRFCMACAES192",
			//																												"KDF108_PIPEModePRFCMACAES256",
			//																												"KDF108_PIPEModePRFCMACTDES2",
			//																												"KDF108_PIPEModePRFCMACTDES3",
			//																												"KDF108_PIPEModePRFHMACSHA1",
			//																												"KDF108_PIPEModePRFHMACSHA224",
			//																												"KDF108_PIPEModePRFHMACSHA256",
			//																												"KDF108_PIPEModePRFHMACSHA384",
			//																												"KDF108_PIPEModePRFHMACSHA512",
			//																												"KDF108_PIPEModeCtrUsed",
			//																												"KDF108_PIPEModeCtrB4AarrayVar",
			//																												"KDF108_PIPEModeCtrAftAarrayVar",
			//																												"KDF108_PIPEModeCtrAftFixedVar",
			//																												"KDF108_PIPEModeRlen8",
			//																												"KDF108_PIPEModeRlen16",
			//																												"KDF108_PIPEModeRlen24",
			//																												"KDF108_PIPEModeRlen32"
			//																												}));

			////Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			//return algorithms.Where(x => x != null).ToList();
		}
	}
}