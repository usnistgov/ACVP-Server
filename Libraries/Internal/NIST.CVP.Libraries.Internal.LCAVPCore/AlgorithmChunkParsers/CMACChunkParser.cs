using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class CMACChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public CMACChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//Have to deal with a CAVS quirk here, you can select the top level things but then not select any children, leading to an empty registration. Check one level deeper before adding the algo


			InfAlgorithm algo = ParseAlgorithmFromModes("AES-CMAC", new List<string> { "CMACGen_AES", "CMACVer_AES" }, new List<string> { "CMAC_PrerequisiteSet", "AES_CMAC_Core_Assurance", "AES_CMAC_Prerequisite_AES", "CMACGen_AES", "CMACGen_AES128", "CMACGen_AES128_(K_EQ_0)", "CMACGen_AES128_(K_mod_B_EQ_0)_1", "CMACGen_AES128_(K_mod_B_EQ_0)_2", "CMACGen_AES128_(K_mod_B<>0)_1", "CMACGen_AES128_(K_mod_B<>0)_2", "CMACGen_AES128_(KMAX_EQ_2^16)", "CMACGen_AES128_KMAX", "CMACGen_AES128_TlenMin", "CMACGen_AES128_TlenMid", "CMACGen_AES128_TlenMax", "CMACGen_AES192", "CMACGen_AES192_(K_EQ_0)", "CMACGen_AES192_(K_mod_B_EQ_0)_1", "CMACGen_AES192_(K_mod_B_EQ_0)_2", "CMACGen_AES192_(K_mod_B<>0)_1", "CMACGen_AES192_(K_mod_B<>0)_2", "CMACGen_AES192_(KMAX_EQ_2^16)", "CMACGen_AES192_KMAX", "CMACGen_AES192_TlenMin", "CMACGen_AES192_TlenMid", "CMACGen_AES192_TlenMax", "CMACGen_AES256", "CMACGen_AES256_(K_EQ_0)", "CMACGen_AES256_(K_mod_B_EQ_0)_1", "CMACGen_AES256_(K_mod_B_EQ_0)_2", "CMACGen_AES256_(K_mod_B<>0)_1", "CMACGen_AES256_(K_mod_B<>0)_2", "CMACGen_AES256_(KMAX_EQ_2^16)", "CMACGen_AES256_KMAX", "CMACGen_AES256_TlenMin", "CMACGen_AES256_TlenMid", "CMACGen_AES256_TlenMax", "CMACVer_AES", "CMACVer_AES128", "CMACVer_AES128_(K_EQ_0)", "CMACVer_AES128_(K_mod_B_EQ_0)_1", "CMACVer_AES128_(K_mod_B_EQ_0)_2", "CMACVer_AES128_(K_mod_B<>0)_1", "CMACVer_AES128_(K_mod_B<>0)_2", "CMACVer_AES128_(KMAX_EQ_2^16)", "CMACVer_AES128_KMAX", "CMACVer_AES128_TlenMin", "CMACVer_AES128_TlenMid", "CMACVer_AES128_TlenMax", "CMACVer_AES192", "CMACVer_AES192_(K_EQ_0)", "CMACVer_AES192_(K_mod_B_EQ_0)_1", "CMACVer_AES192_(K_mod_B_EQ_0)_2", "CMACVer_AES192_(K_mod_B<>0)_1", "CMACVer_AES192_(K_mod_B<>0)_2", "CMACVer_AES192_(KMAX_EQ_2^16)", "CMACVer_AES192_KMAX", "CMACVer_AES192_TlenMin", "CMACVer_AES192_TlenMid", "CMACVer_AES192_TlenMax", "CMACVer_AES256", "CMACVer_AES256_(K_EQ_0)", "CMACVer_AES256_(K_mod_B_EQ_0)_1", "CMACVer_AES256_(K_mod_B_EQ_0)_2", "CMACVer_AES256_(K_mod_B<>0)_1", "CMACVer_AES256_(K_mod_B<>0)_2", "CMACVer_AES256_(KMAX_EQ_2^16)", "CMACVer_AES256_KMAX", "CMACVer_AES256_TlenMin", "CMACVer_AES256_TlenMid", "CMACVer_AES256_TlenMax" });
			if (algo != null && (algo.Options.GetValue("CMACGen_AES128") == "True"
								|| algo.Options.GetValue("CMACGen_AES192") == "True"
								|| algo.Options.GetValue("CMACGen_AES256") == "True"
								|| algo.Options.GetValue("CMACVer_AES128") == "True"
								|| algo.Options.GetValue("CMACVer_AES192") == "True"
								|| algo.Options.GetValue("CMACVer_AES256") == "True")) algorithms.Add(algo);

			//Might be able to drop the TDES modes to just CMACGen_TDES and CMACVer_TDES, but this is safer. CAVS is not entirely clear if they're actually wrappers for everything
			algo = ParseAlgorithmFromModes("TDES-CMAC", new List<string> { "CMACGen_TDES", "CMACGen_TDES2", "CMACGen_TDES3", "CMACVer_TDES", "CMACVer_TDES2", "CMACVer_TDES3" }, new List<string> { "CMAC_PrerequisiteSet", "TDES_CMAC_Core_Assurance", "TDES_CMAC_Prerequisite_TDES", "CMACGen_TDES", "CMACGen_TDES2", "CMACGen_TDES2_(K_EQ_0)", "CMACGen_TDES2_(K_mod_B_EQ_0)_1", "CMACGen_TDES2_(K_mod_B_EQ_0)_2", "CMACGen_TDES2_(K_mod_B<>0)_1", "CMACGen_TDES2_(K_mod_B<>0)_2", "CMACGen_TDES2_(KMAX_EQ_2^16)", "CMACGen_TDES2_KMAX", "CMACGen_TDES2_TlenMin", "CMACGen_TDES2_TlenMid", "CMACGen_TDES2_TlenMax", "CMACGen_TDES3", "CMACGen_TDES3_(K_EQ_0)", "CMACGen_TDES3_(K_mod_B_EQ_0)_1", "CMACGen_TDES3_(K_mod_B_EQ_0)_2", "CMACGen_TDES3_(K_mod_B<>0)_1", "CMACGen_TDES3_(K_mod_B<>0)_2", "CMACGen_TDES3_(KMAX_EQ_2^16)", "CMACGen_TDES3_KMAX", "CMACGen_TDES3_TlenMin", "CMACGen_TDES3_TlenMid", "CMACGen_TDES3_TlenMax", "CMACVer_TDES", "CMACVer_TDES2", "CMACVer_TDES2_(K_EQ_0)", "CMACVer_TDES2_(K_mod_B_EQ_0)_1", "CMACVer_TDES2_(K_mod_B_EQ_0)_2", "CMACVer_TDES2_(K_mod_B<>0)_1", "CMACVer_TDES2_(K_mod_B<>0)_2", "CMACVer_TDES2_(KMAX_EQ_2^16)", "CMACVer_TDES2_KMAX", "CMACVer_TDES2_TlenMin", "CMACVer_TDES2_TlenMid", "CMACVer_TDES2_TlenMax", "CMACVer_TDES3", "CMACVer_TDES3_(K_EQ_0)", "CMACVer_TDES3_(K_mod_B_EQ_0)_1", "CMACVer_TDES3_(K_mod_B_EQ_0)_2", "CMACVer_TDES3_(K_mod_B<>0)_1", "CMACVer_TDES3_(K_mod_B<>0)_2", "CMACVer_TDES3_(KMAX_EQ_2^16)", "CMACVer_TDES3_KMAX", "CMACVer_TDES3_TlenMin", "CMACVer_TDES3_TlenMid", "CMACVer_TDES3_TlenMax" });
			if (algo != null && (algo.Options.GetValue("CMACGen_TDES3") == "True"
								|| algo.Options.GetValue("CMACVer_TDES2") == "True"
								|| algo.Options.GetValue("CMACVer_TDES3") == "True")) algorithms.Add(algo);

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}