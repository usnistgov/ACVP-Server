using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.SHA2.SHAProperties;
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
        /// Processes and adds padding to a message to be valid for SHA1 or SHA2
        /// </summary>
        /// <param name="message">Message to be padded</param>
        /// <returns>BitString paddedMessage</returns>
        public BitString PreProcessing(BitString message)
        {
            var messageLength = message.BitLength;
            message = BitString.ConcatenateBits(message, new BitString("80", 1));

            var bitsNeeded = ((((SHAProperties.BlockSize - SHAProperties.AppendedLength) - message.BitLength) % SHAProperties.BlockSize) + SHAProperties.BlockSize) % SHAProperties.BlockSize;
            message = BitString.ConcatenateBits(message, new BitString(bitsNeeded));

            var messageLengthBS = new BitString(new BigInteger(messageLength), SHAProperties.AppendedLength);
            message = BitString.ConcatenateBits(message, messageLengthBS);

            return message;
        }

        /// <summary>
        /// Divides the padded message into specified length chunks, either 512 or 1024 bits
        /// </summary>
        /// <param name="paddedMessage">Padded message to be broken up into chunks</param>
        /// <returns>BitString[] of evenly sized chunks</returns>
        public BitString[] Chunkify(BitString paddedMessage)
        {
            // Split padded message into 512 or 1024-bit chunks
            var numChunks = paddedMessage.BitLength / SHAProperties.BlockSize;
            var chunks = new BitString[numChunks];

            for (var i = 0; i < numChunks; i++)
            {
                chunks[i] = paddedMessage.MSBSubstring(i * SHAProperties.BlockSize, SHAProperties.BlockSize);
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
