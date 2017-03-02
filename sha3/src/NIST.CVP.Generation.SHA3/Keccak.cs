using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.SHA3
{
    public class Keccak
    {
        private readonly int _b = 1600;
        private readonly int _numRounds = 24;

        /// <summary>
        /// pad10*1
        /// </summary>
        /// <param name="message">Message to which to add padding</param>
        /// <param name="x">Positive integer, rate of sponge function</param>
        /// <returns></returns>
        public BitString PadMessage(BitString message, int x)
        {
            var m = message.BitLength;
            var j = ((-1 * m - 2) % x + x) % x;
            var zeros = new BitString(j);           // Works just fine if j == 0

            // 1 || 0^j || 1
            var padding = BitString.ConcatenateBits(BitString.One(), BitString.ConcatenateBits(zeros, BitString.One()));

            return BitString.ConcatenateBits(message, padding);
        }

        /// <summary>
        /// Sponge function for Keccak. Gathers content for sponge and performs hash
        /// </summary>
        /// <param name="message">Raw BitString message</param>
        /// <param name="digestSize">Digest Size</param>
        /// <returns>Message digest</returns>
        public BitString Sponge(BitString message, int digestSize)
        {
            // Define properties
            var c = digestSize * 2;
            var rate = _b - c;

            // Pad the message
            var paddedMessage = PadMessage(message, rate);

            // Split up padded message into rate chunks
            var n = paddedMessage.BitLength / rate;
            var P = new BitString[n];
            for (var i = 0; i < n; i++)
            {
                P[i] = BitString.MSBSubstring(paddedMessage, i * rate, rate);
            }

            // Build sponge
            var sponge = new BitString(_b);
            for (var i = 0; i < n; i++)
            {
                // This is kinda gross... The message input P[i] is in the correct byte order but reversed bit order
                // So we must reverse the bits, then reverse the bytes to put everything in the correct order
                //
                // For a small example... 60 01 (hex) = 0110 0001 (binary)
                //    should turn into    06 80 (hex) = 0110 1000 (binary)

                var reversedBits = MsbLsbConversionHelpers.ReverseBitArrayBits(P[i].Bits);
                var normalizedBits = MsbLsbConversionHelpers.ReverseByteOrder(new BitString(reversedBits).ToBytes());
                var expectedMessage = new BitString(normalizedBits);

                var spongeContent = BitString.XOR(sponge, BitString.ConcatenateBits(expectedMessage, new BitString(c)));
                sponge = Keccak_p(spongeContent);
            }

            // Truncate output
            var Z = new BitString(0);
            while (true)
            {
                Z = BitString.ConcatenateBits(Z, BitString.MSBSubstring(sponge, 0, rate));
                if (digestSize <= Z.BitLength)
                {
                    return BitString.MSBSubstring(Z, 0, digestSize);
                }

                sponge = Keccak_p(sponge);
            }
        }

        /// <summary>
        /// Performs Keccak-p on the specified message as defined by FIPS-202 Section 3. 
        /// </summary>
        /// <param name="message">BitString to transform. Should already be processed by Keccak.</param>
        /// <returns>BitString</returns>
        public BitString Keccak_p(BitString message)
        {
            var A = new KeccakState(message, _b);

            var startRound = 12 + 2 * A.L - _numRounds;
            var endRound = 12 + 2 * A.L - 1;

            // For A.L = 6 and _numRounds = 24 which they must be for now, this loop is just... (i = 0; i <= 23; i++)
            for (var i = startRound; i <= endRound; i++)
            {
                A = Round(A, i);
            }

            return A.ToBitString();
        }

        public KeccakState Round(KeccakState A, int roundNumber)
        {
            return KeccakState.Iota(KeccakState.Chi(KeccakState.Pi(KeccakState.Rho(KeccakState.Theta(A)))), roundNumber);
        }
    }
}
