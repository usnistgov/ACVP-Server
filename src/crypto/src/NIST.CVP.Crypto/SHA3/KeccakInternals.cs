using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using System;

namespace NIST.CVP.Crypto.SHA3
{
    public class KeccakInternals
    {
        private static int _b = 1600;
        private static int _numRounds = 24;

        private static BitString ConvertEndianness(BitString message)
        {
            // This is kinda gross... The message input is in the correct byte order but reversed bit order
            // So we must reverse the bits, then reverse the bytes to put everything in the correct order
            //
            // For a small example... 60 01 (hex) = 0110 0001 (binary)
            //    should turn into    06 80 (hex) = 0110 1000 (binary

            var messageLen = message.BitLength;

            // Convert to big endian byte order but little endian bit order
            var reversedBits = MsbLsbConversionHelpers.ReverseBitArrayBits(message.Bits);
            var normalizedBits = MsbLsbConversionHelpers.ReverseByteOrder(new BitString(reversedBits).ToBytes());

            // After the byte conversion make sure the result is the correct length
            // The constructor here handles this for us
            message = new BitString(normalizedBits);
            var hex = message.ToHex();
            message = new BitString(hex, messageLen, false);

            return message;
        }

        // Use this method when you need a Little Endian substring that is not a multiple of 8 bits in length
        // For the last byte, we would need to pull bits from the other end of the byte, rather than like reading an array in order
        // This only occurs once under SHAKE with variable output sizes
        private static BitString LittleEndianSubstring(BitString message, int startIdx, int length)
        {
            var lastFullByte = (length / 8) * 8;                                    // Integer division rounds down for us
            var firstBytes = BitString.MSBSubstring(message, startIdx, lastFullByte);

            if (length == lastFullByte)
            {
                return firstBytes;
            }

            var nextByte = BitString.MSBSubstring(message, startIdx + lastFullByte, 8);

            var bitsNeeded = length % 8;
            var lastBits = new BitString(0);
            if (bitsNeeded != 0)
            {
                lastBits = BitString.Substring(nextByte, 0, bitsNeeded);
            }

            return BitString.ConcatenateBits(firstBytes, lastBits);
        }

        /// <summary>
        /// External Keccak function. This is the method to call.
        /// </summary>
        /// <param name="message">Message to hash</param>
        /// <param name="digestSize">Size of the digest to return</param>
        /// <param name="capacity">Capacity of the function</param>
        /// <param name="outputType">XOF for SHAKE, CONSTANT for SHA3, cXOF for cSHAKE</param>
        /// <param name="cSHAKEPrePad">True if cSHAKE had customization parameters other than ""</param>
        /// <returns>Message digest as BitString</returns>
        public static BitString Keccak(BitString message, int digestSize, int capacity, Output outputType, Boolean cSHAKEPrePad = false)
        {
            message = ConvertEndianness(message);

            if (!cSHAKEPrePad && outputType == Output.cXOF)
            {
                message = BitString.ConcatenateBits(message, BitString.Ones(4));
            }
            else if (outputType == Output.XOF)
            {
                message = BitString.ConcatenateBits(message, BitString.Ones(4));
            }
            else if (outputType == Output.CONSTANT)
            {
                message = BitString.ConcatenateBits(message, BitString.Zero());
                message = BitString.ConcatenateBits(message, BitString.One());
            }

            return Sponge(message, digestSize, capacity);
        }

        /// <summary>
        /// pad10*1
        /// </summary>
        /// <param name="message">Message to which to add padding</param>
        /// <param name="x">Positive integer, rate of sponge function</param>
        /// <returns></returns>
        public static BitString PadMessage(BitString message, int x)
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
        /// <param name="capacity">Capacity of the sponge function</param>
        /// <returns>Message digest</returns>
        public static BitString Sponge(BitString message, int digestSize, int capacity)
        {
            // Define properties
            // All possible rates are divisible by 8
            var rate = _b - capacity;

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
                var P_i = ConvertEndianness(P[i]);

                var spongeContent = BitString.XOR(sponge, BitString.ConcatenateBits(P_i, new BitString(capacity)));
                sponge = Keccak_p(spongeContent);
            }

            // Truncate output
            var Z = new BitString(0);
            while (true)
            {
                Z = BitString.ConcatenateBits(Z, BitString.MSBSubstring(sponge, 0, rate));
                if (digestSize <= Z.BitLength)
                {
                    return LittleEndianSubstring(Z, 0, digestSize);
                }

                sponge = Keccak_p(sponge);
            }
        }

        /// <summary>
        /// Performs Keccak-p on the specified message as defined by FIPS-202 Section 3. 
        /// </summary>
        /// <param name="message">BitString to transform. Should already be processed by Keccak.</param>
        /// <returns>BitString</returns>
        public static BitString Keccak_p(BitString message)
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

        public static KeccakState Round(KeccakState A, int roundNumber)
        {
            return KeccakState.Iota(KeccakState.Chi(KeccakState.Pi(KeccakState.Rho(KeccakState.Theta(A)))), roundNumber);
        }
    }
}
