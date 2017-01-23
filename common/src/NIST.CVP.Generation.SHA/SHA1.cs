using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public class SHA1 : SHA
    {
        // These are all unsigned long (ulong), 32-bits
        private BitString _k0, _k1, _k2, _k3;
        private BitString _h0, _h1, _h2, _h3, _h4;
        private BitString _a, _b, _c, _d, _e;
        private BitString[] _wBuffer;

        public override BitString HashMessage(HashFunction hashFunction, BitString message)
        {
            // We can basically ignore hashFunction because there is only one way to do SHA1...

            // Set up blocks
            InitialSetUp();
            var paddedMessage = PreProcessing(message);
            var chunks = Chunkify(paddedMessage);

            // Process each block
            foreach (var chunk in chunks)
            {
                DivideChunk(chunk);
                ProcessBlock();
            }

            // Build result
            var result = BitString.ConcatenateBits(_h0, _h1);
            result = BitString.ConcatenateBits(result, _h2);
            result = BitString.ConcatenateBits(result, _h3);
            result = BitString.ConcatenateBits(result, _h4);

            return result;
        }

        // Private Functions
        #region Helpers
        private void InitialSetUp()
        {
            _k0 = new BitString("5A827999");
            _k1 = new BitString("6ED9EBA1");
            _k2 = new BitString("8F1BBCDC");
            _k3 = new BitString("CA62C1D6");

            _h0 = new BitString("67452301");
            _h1 = new BitString("EFCDAB89");
            _h2 = new BitString("98BADCFE");
            _h3 = new BitString("10325476");
            _h4 = new BitString("C3D2E1F0");
        }

        private BitString PreProcessing(BitString message)
        {
            // Assume initial message is BigEndian and shorter than 2^64 bits. 

            var messageLength = message.BitLength;
            if (messageLength % 8 == 0)
            {
                // Append the bit '1' to the end of message
                message = BitString.ConcatenateBits(message, new BitString("80"));
            }

            // Pad message until length is -64 mod 512, (or 448 mod 512).
            var bitsNeeded = (448 - message.BitLength) % 512;

            // Constructor defaults to a BitString of bitsNeeded length full of 0s
            message = BitString.ConcatenateBits(message, new BitString(bitsNeeded));

            // Append original message length (as a 64-bit integer) to the message
            var messageLengthBS = new BitString(new BigInteger(messageLength), 64);
            message = BitString.ConcatenateBits(message, messageLengthBS);

            // This ensures BitLength is 0 mod 512 for even chunks
            return message;
        }

        private BitString[] Chunkify(BitString paddedMessage)
        {
            // Split padded message into 512-bit chunks
            var numChunks = paddedMessage.BitLength / 512;
            var chunks = new BitString[numChunks];

            for (var i = 0; i < numChunks; i++)
            {
                chunks[i] = paddedMessage.Substring((numChunks-i-1) * 512, 512);
            }

            return chunks;
        }

        private void DivideChunk(BitString chunk)
        {
            _wBuffer = new BitString[80];

            // Split each chunk into 16, 32-bit words
            for (var i = 0; i < 16; i++)
            {
                _wBuffer[i] = chunk.Substring((16-i-1) * 32, 32);
            }

            // The rest of the words are an expansion of the previous
            for (var i = 16; i < 80; i++)
            {
                var intermediate = _wBuffer[i - 3].GetDeepCopy();
                intermediate = BitString.XOR(intermediate, _wBuffer[i - 8]);
                intermediate = BitString.XOR(intermediate, _wBuffer[i - 14]);
                intermediate = BitString.XOR(intermediate, _wBuffer[i - 16]);

                _wBuffer[i] = CS1(intermediate);
            }
        }

        private void ProcessBlock()
        {
            _a = _h0.GetDeepCopy();
            _b = _h1.GetDeepCopy();
            _c = _h2.GetDeepCopy();
            _d = _h3.GetDeepCopy();
            _e = _h4.GetDeepCopy();

            // Perform Rounds
            for (var i = 0; i < 20; i++)
            {
                Round1(i);
            }

            for (var i = 20; i < 40; i++)
            {
                Round2(i);
            }

            for (var i = 40; i < 60; i++)
            {
                Round3(i);
            }

            for (var i = 60; i < 80; i++)
            {
                Round4(i);
            }

            // Increment H Values wrapped to remain within 2^32
            _h0 = BitString.AddWithModulo(_h0, _a, 32);
            _h1 = BitString.AddWithModulo(_h1, _b, 32);
            _h2 = BitString.AddWithModulo(_h2, _c, 32);
            _h3 = BitString.AddWithModulo(_h3, _d, 32);
            _h4 = BitString.AddWithModulo(_h4, _e, 32);
        }
        #endregion Helpers

        #region F Functions
        private BitString F1(BitString x, BitString y, BitString z)
        {
            // (x & (y ^ z)) ^ z
            var copyX = x.GetDeepCopy();
            var copyY = y.GetDeepCopy();
            var copyZ = z.GetDeepCopy();
            return BitString.XOR(BitString.AND(copyX, BitString.XOR(copyY, copyZ)), copyZ);
        }

        private BitString F2(BitString x, BitString y, BitString z)
        {
            // x ^ y ^ z
            var copyX = x.GetDeepCopy();
            var copyY = y.GetDeepCopy();
            var copyZ = z.GetDeepCopy();
            return BitString.XOR(BitString.XOR(copyX, copyY), copyZ);
        }

        private BitString F3(BitString x, BitString y, BitString z)
        {
            // (x & y) | (x & z) | (y & z)
            var firstAND = BitString.AND(x.GetDeepCopy(), y.GetDeepCopy());
            var secondAND = BitString.AND(x.GetDeepCopy(), z.GetDeepCopy());
            var thirdAND = BitString.AND(y.GetDeepCopy(), z.GetDeepCopy());
            return BitString.OR(firstAND, BitString.OR(secondAND, thirdAND));
        }
        #endregion F Functions

        #region Rounds
        private void Round1(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F1(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _wBuffer[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k0, 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round2(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F2(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _wBuffer[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k1, 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round3(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F3(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _wBuffer[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k2, 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round4(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F2(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _wBuffer[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k3, 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }
        #endregion Rounds

        #region Circular Shifts
        private BitString CS1(BitString x)
        {
            return BitString.CircularShiftMSB(x, 1);
        }

        private BitString CS5(BitString x)
        {
            return BitString.CircularShiftMSB(x, 5);
        }

        private BitString CS30(BitString x)
        {
            return BitString.CircularShiftMSB(x, 30);
        }
        #endregion
    }
}
