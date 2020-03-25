using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class GCMChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public GCMChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//GCM can result in GCM and/or GMAC, depending on the PT length values. So first build a dummy algo. We'll reuse its parts in a moment
			var algo = ParseAlgorithmFromMode("GCM or GMAC", "GCMtested", Chunk.KeyValuePairs.Select(x => x.Key).Except(new List<string> { "GCMtested", "XPNtested", "XPNInternalSalt" }).ToList());

			//If "GCM" was tested we got something back, so now figure out if it is GCM and/or GMAC
			if (algo != null)
			{
				//Create a GMAC instance if PTlength0 = true
				if (algo.Options.GetValue("PTlength0") == "True")
				{
					algorithms.Add(new InfAlgorithm
					{
						AlgorithmName = "AES-GMAC",
						Options = algo.Options,
						Prerequisites = algo.Prerequisites
					});
				}

				//Create a GCM instance if any of the other 4 PT length related fields have values
				if (algo.Options.GetValue("MinPTLength") != "0"
					|| algo.Options.GetValue("MaxPTLength") != "0"
					|| algo.Options.GetValue("MinPTLengthNonMult") != "0"
					|| algo.Options.GetValue("MaxPTLengthNonMult") != "0")
				{
					algorithms.Add(new InfAlgorithm
					{
						AlgorithmName = "AES-GCM",
						Options = algo.Options,
						Prerequisites = algo.Prerequisites
					});
				}
			}

			//If tested, XPN needs everything but whether GCM was tested, Keysize_192, and some IV stuff (since must use 96 bit)
			algorithms.Add(ParseAlgorithmFromMode("AES-XPN", "XPNtested", Chunk.KeyValuePairs.Select(x => x.Key).Except(new List<string> { "GCMtested", "Keysize_192", "OtherIVlen", "MinOtherIVlen", "MaxOtherIVlen" }).ToList()));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}