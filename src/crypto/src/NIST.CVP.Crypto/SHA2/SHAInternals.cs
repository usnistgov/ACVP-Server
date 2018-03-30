using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Hash.SHA2.SHAProperties;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHAInternals
    {
        public SHAPropertiesBase SHAProperties { get; }

        public SHAInternals(HashFunction hashFunction)
        {
            var propertyFactory = new SHAPropertyFactory();
            SHAProperties = propertyFactory.GetSHAProperties(hashFunction);
        }

        /// <summary>
        /// Divides the padded message into specified length chunks, either 512 or 1024 bits
        /// </summary>
        /// <param name="paddedMessage">Padded message to be broken up into chunks</param>
        /// <returns>BitString[] of evenly sized chunks</returns>
        public BitString[] Chunkify(BitString message)
        {
            var numChunks = message.BitLength / SHAProperties.BlockSize;
            var chunks = new BitString[numChunks];

            for (var i = 0; i < numChunks; i++)
            {
                chunks[i] = message.MSBSubstring(i * SHAProperties.BlockSize, SHAProperties.BlockSize);
            }

            return chunks;
        }

        /// <summary>
        /// Builds the result from the H values
        /// </summary>
        /// <param name="hValues">Array of H values from the rounds</param>
        /// <returns>BitString of the digest</returns>
        public BitString BuildResult(BitString[] hValues)
        {
            var result = new BitString(0);
            for(var i = 0; i < hValues.Count(); i++)
            {
                result = BitString.ConcatenateBits(result, hValues[i]);
            }

            return result.MSBSubstring(0, SHAProperties.DigestBitSize);
        }
    }
}
