using System;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers
{
    public static class LmsHelpers
    {
        public static readonly byte[] D_PBLC = 0x8080.Get16Bits();
        public static readonly byte[] D_MESG = 0x8181.Get16Bits();
        public static readonly byte[] D_LEAF = 0x8282.Get16Bits();
        public static readonly byte[] D_INTR = 0x8383.Get16Bits();
        public static readonly byte[] Zeroes_Four_bytes = new byte[4];
        public static readonly byte[] FF = { 0xFF };
        public static readonly byte[] FFFE = 0xFFFE.Get16Bits();
        public static readonly byte[] FFFF = 0xFFFF.Get16Bits();

        /// <summary>
        /// Coef function from https://datatracker.ietf.org/doc/html/rfc8554#section-3.1.3
        ///
        /// coef(S, i, w) = (2^w - 1) AND
        ///               ( byte(S, floor(i * w / 8)) >>
        ///                 (8 - (w * (i % (8 / w)) + w)) )
        /// </summary>
        /// <param name="s">byte array</param>
        /// <param name="i">byte index</param>
        /// <param name="w">number of the set { 1, 2, 4, 8 }.</param>
        /// <returns></returns>
        public static BigInteger Coef(byte[] s, int i, int w)
        {
            var lhs = (BigInteger.One << w) - 1;

            var rhs = new BigInteger(s[(i * w).FloorDivide(8)] >> (8 - (w * (i % (8 / w)) + w)));

            return lhs & rhs;
        }

        /// <summary>
        /// Checksum function to detect forgery attempts from  https://datatracker.ietf.org/doc/html/rfc8554#section-4.4
        /// </summary>
        /// <param name="s">The n-byte string to checksum.</param>
        /// <param name="n">The byte length of the string <see cref="s"/>.</param>
        /// <param name="w">the number of bits from the hash or checksum used in a single Winternitz chain.</param>
        /// <param name="ls">The left shift calculation for the LM-OTS parameter set.</param>
        /// <returns>The checksum.</returns>
        public static int Checksum(byte[] s, int n, int w, int ls)
        {
            /*
			 sum = 0
		     for ( i = 0; i < (n*8/w); i = i + 1 ) {
		       sum = sum + (2^w - 1) - coef(S, i, w)
		     }
		     return (sum << ls)
			 */

            var sum = BigInteger.Zero;
            for (var i = 0; i < n * 8 / w; i++)
            {
                sum += BigInteger.Pow(2, w) - 1 - Coef(s, i, w);
            }

            return (int)sum << ls;
        }

        /// <summary>
        /// The generation of Q along with the Q Checksum.
        /// </summary>
        /// <param name="i">The 16 byte LMS tree identifier.</param>
        /// <param name="q">The LMS leaf identifier.</param>
        /// <param name="c">The randomizer string to add to the signature.</param>
        /// <param name="message">The message to sign.</param>
        /// <param name="attribute">The LM-OTS attribute.</param>
        /// <param name="sha">An instance of SHA used for computation.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The Q value concatenated with its checksum.</returns>
        public static byte[] QChecksumConcatenation(byte[] i, byte[] q, byte[] c, byte[] message,
            LmOtsAttribute attribute, ISha sha, byte[] buffer)
        {
            // Q = H(I || u32str(q) || u16str(D_MESG) || C || message)
            var Q = new byte[attribute.N];
            sha.Init();
            sha.Update(i, i.BitLength());
            sha.Update(q, q.BitLength());
            sha.Update(D_MESG, 16);
            sha.Update(c, c.BitLength());
            sha.Update(message, message.BitLength());
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, Q, Q.Length);

            var checksum = Checksum(Q, attribute.N, attribute.W, attribute.LeftShift).Get16Bits();

            var qChecksumConcatenation = new byte[Q.Length + checksum.Length];

            Array.Copy(Q, qChecksumConcatenation, Q.Length);
            Array.Copy(checksum, 0, qChecksumConcatenation, Q.Length, checksum.Length);
            return qChecksumConcatenation;
        }

        /// <summary>
        /// Compute the LM-OTS public key candidate from a signature.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-4.6
        /// Algorithm 4b
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The LM-OTS attributes associated to the signature.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="message">The purported message that was signed to create <see cref="signature"/></param>
        /// <param name="i">The 16 byte LMS tree identifier.</param>
        /// <param name="q">The 4 byte LMS leaf identifier.</param>
        /// <returns>The key candidate after running through Algorithm 4a.</returns>
        /// <exception cref="ArgumentException">Thrown when the signature length is not appropriate to the parameters given.</exception>
        public static byte[] GetLmOtsPublicKeyCandidate(ISha sha, LmOtsAttribute attribute, byte[] signature, byte[] message, byte[] i, byte[] q)
        {
            if (signature.Length < 4)
                throw new ArgumentException($"{nameof(signature)} must be at least 4 bytes.");

            var sigType = signature.Take(4).ToArray();
            var sigMode = AttributesHelper.GetLmOtsModeFromTypeCode(sigType);

            if (sigMode != attribute.Mode)
                throw new ArgumentException("sigType not equal to pubType.");

            var expectedSignatureLength = 4 + attribute.N * (attribute.P + 1);
            if (signature.Length != expectedSignatureLength)
                throw new ArgumentException(
                    $"{nameof(signature)} was expected to be {expectedSignatureLength} bytes, was {signature.Length}.");

            var c = signature.Skip(4).Take(attribute.N).ToArray();
            var y = new byte[attribute.P][];
            for (var j = 0; j < attribute.P; j++)
            {
                y[j] = signature
                    .Skip(4 + attribute.N + (attribute.N * j))
                    .Take(attribute.N)
                    .ToArray();
            }

            var bufferSizeBytes = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode);
            var buffer = new byte[bufferSizeBytes];

            var qChecksumConcatenation =
                QChecksumConcatenation(i, q, c, message, attribute, sha, buffer);

            for (var j = 0; j < attribute.P; j++)
            {
                var a = (int)Coef(qChecksumConcatenation, j, attribute.W);
                var tmp = y[j];
                for (var k = a; k < BigInteger.Pow(2, attribute.W) - 1; k++)
                {
                    // tmp = H(I || u32str(q) || u16str(i) || u8str(j) || tmp)
                    sha.Init();
                    sha.Update(i, i.BitLength());
                    sha.Update(q, q.BitLength());
                    sha.Update(j, 16);
                    sha.Update(k, 8);
                    sha.Update(tmp, tmp.BitLength());
                    sha.Final(buffer, buffer.BitLength());

                    Array.Copy(buffer, tmp, tmp.Length);
                }

                // this is `z` in the pseudocode, but there's seemingly no reason to allocate a new array.
                y[j] = tmp;
            }

            // Kc = H(I || u32str(q) || u16str(D_PBLC) || z[0] || z[1] || ... || z[p-1])
            var kc = new byte[attribute.N];
            sha.Init();
            sha.Update(i, i.BitLength());
            sha.Update(q, q.BitLength());
            sha.Update(D_PBLC, 16);
            for (var j = 0; j < y.Length; j++)
            {
                sha.Update(y[j], y[j].BitLength());
            }
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, kc, kc.Length);

            return kc;
        }

        /// <summary>
        /// Mutates a given <see cref="seed"/> and <see cref="i"/> value to something new and unique.
        ///
        /// Can be used for children LMS trees, or the recreation of a tree that has been exhausted.
        /// </summary>
        /// <param name="sha">The <see cref="ISha"/> instance used for generating the new i/seed values.</param>
        /// <param name="lmsAttribute">The attributes of the LMS tree.</param>
        /// <param name="seed">The original M-byte seed.</param>
        /// <param name="i">The original 16-byte LMS tree identifier.</param>
        /// <param name="level">The HSS level in which the seed/id are being generated.</param>
        /// <returns><see cref="IdSeedResult"/>.</returns>
        public static IdSeedResult CalculateNewSeedIdFromExisting(ISha sha, LmsAttribute lmsAttribute, byte[] i, byte[] seed, int level)
        {
            var bufferLength = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmsAttribute.Mode);
            var buffer = new byte[bufferLength];

            var newSeed = new byte[lmsAttribute.M];
            var newI = new byte[16];

            var levelBytes = level.GetBytes();

            // newSeed = H(I || 0x00000000 || 0xFFFE || 0xFF || seed || level as 32 bits)
            sha.Init();
            sha.Update(i, i.BitLength());
            sha.Update(Zeroes_Four_bytes, 32);
            sha.Update(FFFE, 16);
            sha.Update(FF, 8);
            sha.Update(seed, seed.BitLength());
            sha.Update(levelBytes, 32);
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, newSeed, newSeed.Length);

            // newI = H(I || 0x00000000 || 0xFFFF || FF || msb(seed, 128) || level as 32 bits)
            sha.Init();
            sha.Update(i, i.BitLength());
            sha.Update(Zeroes_Four_bytes, 32);
            sha.Update(FFFF, 16);
            sha.Update(FF, 8);
            sha.Update(seed, 128);
            sha.Update(levelBytes, 32);
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, newI, newI.Length);

            return new IdSeedResult(newI, newSeed);
        }

        /// <summary>
        /// Gets the appropriate SHA instance based on the <see cref="LmOtsMode"/>.
        /// </summary>
        /// <param name="shaFactory">The <see cref="IShaFactory"/> to retrieve an instance from.</param>
        /// <param name="mode">The <see cref="LmOtsMode"/> being used for the construction of a <see cref="ILmOtsKeyPair"/>.</param>
        /// <returns>An instance of <see cref="ISha"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <see cref="LmOtsMode"/> is invalid.</exception>
        public static ISha GetSha(IShaFactory shaFactory, LmOtsMode mode)
        {
            switch (mode)
            {
                case LmOtsMode.LMOTS_SHA256_N24_W1:
                case LmOtsMode.LMOTS_SHA256_N24_W2:
                case LmOtsMode.LMOTS_SHA256_N24_W4:
                case LmOtsMode.LMOTS_SHA256_N24_W8:
                case LmOtsMode.LMOTS_SHA256_N32_W1:
                case LmOtsMode.LMOTS_SHA256_N32_W2:
                case LmOtsMode.LMOTS_SHA256_N32_W4:
                case LmOtsMode.LMOTS_SHA256_N32_W8:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                case LmOtsMode.LMOTS_SHAKE_N24_W1:
                case LmOtsMode.LMOTS_SHAKE_N24_W2:
                case LmOtsMode.LMOTS_SHAKE_N24_W4:
                case LmOtsMode.LMOTS_SHAKE_N24_W8:
                case LmOtsMode.LMOTS_SHAKE_N32_W1:
                case LmOtsMode.LMOTS_SHAKE_N32_W2:
                case LmOtsMode.LMOTS_SHAKE_N32_W4:
                case LmOtsMode.LMOTS_SHAKE_N32_W8:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), $"Unsupported {nameof(mode)} for retrieving a hash function.");
            }
        }

        /// <summary>
        /// Gets the appropriate SHA instance based on the <see cref="LmsMode"/>.
        /// </summary>
        /// <param name="shaFactory">The <see cref="IShaFactory"/> to retrieve an instance from.</param>
        /// <param name="mode">The <see cref="LmsMode"/> being used for the construction of a <see cref="ILmsKeyPair"/>.</param>
        /// <returns>An instance of <see cref="ISha"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <see cref="LmsMode"/> is invalid.</exception>
        public static ISha GetSha(IShaFactory shaFactory, LmsMode mode)
        {
            switch (mode)
            {
                case LmsMode.LMS_SHA256_M24_H5:
                case LmsMode.LMS_SHA256_M24_H10:
                case LmsMode.LMS_SHA256_M24_H15:
                case LmsMode.LMS_SHA256_M24_H20:
                case LmsMode.LMS_SHA256_M24_H25:
                case LmsMode.LMS_SHA256_M32_H5:
                case LmsMode.LMS_SHA256_M32_H10:
                case LmsMode.LMS_SHA256_M32_H15:
                case LmsMode.LMS_SHA256_M32_H20:
                case LmsMode.LMS_SHA256_M32_H25:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                case LmsMode.LMS_SHAKE_M24_H5:
                case LmsMode.LMS_SHAKE_M24_H10:
                case LmsMode.LMS_SHAKE_M24_H15:
                case LmsMode.LMS_SHAKE_M24_H20:
                case LmsMode.LMS_SHAKE_M24_H25:
                case LmsMode.LMS_SHAKE_M32_H5:
                case LmsMode.LMS_SHAKE_M32_H10:
                case LmsMode.LMS_SHAKE_M32_H15:
                case LmsMode.LMS_SHAKE_M32_H20:
                case LmsMode.LMS_SHAKE_M32_H25:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unsupported {nameof(mode)} for retrieving a hash function.");
            }
        }
    }
}
