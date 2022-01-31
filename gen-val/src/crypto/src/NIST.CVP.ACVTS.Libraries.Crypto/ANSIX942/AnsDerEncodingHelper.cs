using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ANSIX942
{
    public static class AnsDerEncodingHelper
    {
        private static BitString _sequence = new BitString("30");
        private static BitString _octet = new BitString("04");
        private static BitString _partyU = new BitString("a0");
        private static BitString _partyV = new BitString("a1");
        private static BitString _suppPub = new BitString("a2");
        private static BitString _suppPriv = new BitString("a3");

        /// <summary>
        ///  Content will be treated as complete bytes. Sequence || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>Sequence || length || value</returns>
        public static BitString EncodeSequence(BitString content)
        {
            return PerformEncode(_sequence, GetLengthAsByte(content), content);
        }

        /// <summary>
        /// Content will be treated as complete bytes. Octet || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>Octet || length || value</returns>
        public static BitString EncodeOctet(BitString content)
        {
            return PerformEncode(_octet, GetLengthAsByte(content), content);
        }

        /// <summary>
        /// Content will be treated as complete bytes. PartyU || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>PartyU || length || value</returns>
        public static BitString EncodePartyUInfo(BitString content)
        {
            return PerformEncode(_partyU, GetLengthAsByte(content), content);
        }

        /// <summary>
        /// Content will be treated as complete bytes. PartyV || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>PartyV || length || value</returns>
        public static BitString EncodePartyVInfo(BitString content)
        {
            return PerformEncode(_partyV, GetLengthAsByte(content), content);
        }

        /// <summary>
        /// Content will be treated as complete bytes. SuppPub || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>SuppPub || length || value</returns>
        public static BitString EncodeSuppPubInfo(BitString content)
        {
            return PerformEncode(_suppPub, GetLengthAsByte(content), content);
        }

        /// <summary>
        /// Content will be treated as complete bytes. SuppPriv || length || value.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>SuppPriv || length || value</returns>
        public static BitString EncodeSuppPrivInfo(BitString content)
        {
            return PerformEncode(_suppPriv, GetLengthAsByte(content), content);
        }

        private static BitString PerformEncode(BitString label, BitString length, BitString content)
        {
            // Type || length || value
            return BitString.ConcatenateBits(label, BitString.ConcatenateBits(length, content));
        }

        private static BitString GetLengthAsByte(BitString content)
        {
            if (content.BitLength < 1024) // 128 * 8 bits
            {
                return BitString.To8BitString((byte)(content.BitLength / 8));  // To add to below, byte is naturally unsigned, 0 to 255. 
            }
            else if (content.BitLength < 524288) // 65536 * 8 bits
            {
                // Length is a multi-part field, the first bit is set to 1. The next 7 bits communicate the length (bytes) of the upcoming length property
                // Then the actual length of the content is present after.

                var bytesInContent = content.BitLength / 8;
                var length = BitString.PadToNextByteBoundry(((BigInteger)bytesInContent).ExactBitString());
                var bytesNeededForLength = length.BitLength / 8;

                var preamble = BitString.PadToNextByteBoundry(((BigInteger)bytesNeededForLength).ExactBitString());
                preamble.Set(0, true);

                return preamble.ConcatenateBits(length);
            }
            else // We shouldn't be generating DER encoded strings this long
            {
                throw new Exception("Content too large to encode in DER encoder");
            }
        }
    }
}
