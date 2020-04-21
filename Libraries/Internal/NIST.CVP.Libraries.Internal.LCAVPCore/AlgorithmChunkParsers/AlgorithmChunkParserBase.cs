using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class AlgorithmChunkParserBase
	{
		public InfFileSection Chunk { get; protected set; }

		public AlgorithmChunkParserBase(InfFileSection chunk)
		{
			Chunk = chunk;
		}

		/// <summary>
		/// Return the specified lines if the algorithm was tested - in this case, it means in the old world the mode was tested
		/// </summary>
		/// <param name="algorithmName">What we now call the algorithm</param>
		/// <param name="algorithmKey">The name of the "mode" tag</param>
		/// <param name="optionsKeys">The lines we want to return</param>
		/// <returns></returns>
		public InfAlgorithm ParseAlgorithmFromMode(string algorithmName, string algorithmKey, List<string> optionsKeys)
		{
			InfAlgorithm algo = null;

			if (Chunk.KeyValuePairs.Exists(x => x.Key == algorithmKey && x.Value == "True"))
			{
				algo = new InfAlgorithm { AlgorithmName = algorithmName };

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, optionsKeys));
			}

			return algo;
		}

		//Use this when there are mulitple "modes" that trigger the same algorithm
		public InfAlgorithm ParseAlgorithmFromModes(string algorithmName, List<string> algorithmKeys, List<string> optionsKeys)
		{
			InfAlgorithm algo = null;

			if (Chunk.KeyValuePairs.Exists(x => algorithmKeys.Any(s => s == x.Key) && x.Value == "True"))
			{
				algo = new InfAlgorithm { AlgorithmName = algorithmName };

				//Now get the lines that go with it
				algo.Options.Add(ParsingHelper.GetLinesByKeys(Chunk.KeyValuePairs, optionsKeys));
			}

			return algo;
		}
	}
}