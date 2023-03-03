using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public class LmOtsKeyPairFactory : ILmOtsKeyPairFactory
    {
        private readonly IShaFactory _shaFactory;

        public LmOtsKeyPairFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public ILmOtsKeyPair GetKeyPair(LmOtsMode mode, byte[] i, byte[] q, byte[] seed)
        {
            var attribute = AttributesHelper.GetLmOtsAttribute(mode);
            var privateKey = GetPrivateKey(attribute, i, q, seed);
            var publicKey = GetPublicKey(privateKey);

            return new LmOtsKeyPair
            {
                LmOtsAttribute = attribute,
                PrivateKey = privateKey,
                PublicKey = publicKey,
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown when i or q are not of valid lengths.</exception>
        /// <exception cref="ArgumentException">Thrown when seed is not at least <see cref="LmOtsAttribute.N"/> in length.</exception>
        public ILmOtsPrivateKey GetPrivateKey(LmOtsAttribute lmOtsAttribute, byte[] i, byte[] q, byte[] seed)
        {
            if (i?.Length != 16)
            {
                throw new ArgumentOutOfRangeException($"{nameof(i)} was expected to be 16 bytes, was {i?.Length}.");
            }

            if (q?.Length != 4)
            {
                throw new ArgumentOutOfRangeException($"{nameof(q)} was expected to be 4 bytes, was {q?.Length}.");
            }

            if (seed?.Length < lmOtsAttribute.N)
            {
                throw new ArgumentException($"Expected byte array of at least length {lmOtsAttribute.N}, got {seed?.Length}", nameof(seed));
            }
            var x = GetPrivateX(lmOtsAttribute, i, q, seed);
            var key = GetPrivateKey(lmOtsAttribute, i, q, x);

            return new LmOtsPrivateKey(lmOtsAttribute, i, q, seed, x, key);
        }

        private ILmOtsPublicKey GetPublicKey(ILmOtsPrivateKey privateKey)
        {
            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));

            var k = GetPublicK(privateKey.LmOtsAttribute, privateKey);
            var key = GetPublicKey(privateKey.LmOtsAttribute, privateKey, k);

            return new LmOtsPublicKey(privateKey.LmOtsAttribute, k, key);
        }

        private byte[][] GetPrivateX(LmOtsAttribute lmOtsAttribute, byte[] i, byte[] q, byte[] seed)
        {
            var x = new byte[lmOtsAttribute.P][];

            var bufferSizeBytes = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmOtsAttribute.Mode);
            var ff = new byte[] { 255 };
            var sha = LmsHelpers.GetSha(_shaFactory, lmOtsAttribute.Mode);
            var digestBuffer = new byte[bufferSizeBytes];
            for (var j = 0; j < lmOtsAttribute.P; j++)
            {
                var nByte = new byte[lmOtsAttribute.N];
                // x_q[i] = H(I || u32str(q) || u16str(i) || u8str(0xff) || SEED).
                sha.Init();
                sha.Update(i, i.BitLength());
                sha.Update(q, q.BitLength());
                sha.Update(j, 16);
                sha.Update(ff, 8);
                sha.Update(seed, seed.BitLength());
                sha.Final(digestBuffer, digestBuffer.BitLength());

                // Copy the buffer into the nByte array up to the number of bytes required for the chosen N value
                Array.Copy(digestBuffer, nByte, lmOtsAttribute.N);

                x[j] = nByte;
            }

            return x;
        }

        private byte[] GetPrivateKey(LmOtsAttribute lmOtsAttribute, byte[] i, byte[] q, byte[][] x)
        {
            // Allocate the array to accomodate all elements of the key
            var fullKey = new byte[lmOtsAttribute.NumericIdentifier.Length + i.Length + q.Length + x.Select(s => s.Length).Sum()];

            // typeCode | 32 bits / 4 bytes | _fullKey[0]..._fullKey[3]
            Array.Copy(lmOtsAttribute.NumericIdentifier, 0, fullKey, 0, 4);

            // I | 128 bits / 16 bytes | _fullKey[4]..._fullKey[19]
            Array.Copy(i, 0, fullKey, 4, 16);

            // Q | 32 bits / 4 bytes | _fullKey[20]..._fullKey[23]
            Array.Copy(q, 0, fullKey, 20, 4);

            // X[0]||X[1]||...X[p-1] | _fullKey[24]..._fullKey[_fullKey.Length-1]
            var fullKeyStartIndexOfX = 24;
            for (var p = 0; p < x.Length; p++)
            {
                Array.Copy(
                    x[p], 0,
                    fullKey, fullKeyStartIndexOfX + (lmOtsAttribute.N * p),
                    lmOtsAttribute.N);
            }

            return fullKey;
        }

        private byte[] GetPublicK(LmOtsAttribute lmOtsAttribute, ILmOtsPrivateKey privateKey)
        {
            var sha = LmsHelpers.GetSha(_shaFactory, lmOtsAttribute.Mode);
            var bufferSizeBytes = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmOtsAttribute.Mode);
            var buffer = new byte[bufferSizeBytes];
            var y = new byte[lmOtsAttribute.P][];
            var x = privateKey.X;
            for (var i = 0; i < lmOtsAttribute.P; i++)
            {
                var tmp = new byte[lmOtsAttribute.N];
                Array.Copy(x[i], tmp, tmp.Length);

                for (var j = 0; j < System.Math.Pow(2, lmOtsAttribute.W) - 1; j++)
                {
                    // tmp = H(I || u32str(q) || u16str(i) || u8str(j) || tmp)
                    sha.Init();
                    sha.Update(privateKey.I, privateKey.I.BitLength());
                    sha.Update(privateKey.Q, privateKey.Q.BitLength());
                    sha.Update(i, 16);
                    sha.Update(j, 8);
                    sha.Update(tmp, tmp.BitLength());
                    sha.Final(buffer, buffer.BitLength());

                    Array.Copy(buffer, tmp, tmp.Length);
                }

                y[i] = tmp;
            }

            // K = H(I || u32str(q) || u16str(D_PBLC) || y[0] || ... || y[p-1])
            var k = new byte[lmOtsAttribute.N];
            sha.Init();
            sha.Update(privateKey.I, privateKey.I.BitLength());
            sha.Update(privateKey.Q, privateKey.Q.BitLength());
            sha.Update(LmsHelpers.D_PBLC, 16);
            for (var i = 0; i < y.Length; i++)
            {
                sha.Update(y[i], y[i].BitLength());
            }

            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, k, k.Length);

            return k;
        }

        private byte[] GetPublicKey(LmOtsAttribute lmOtsAttribute, ILmOtsPrivateKey privateKey, byte[] k)
        {
            // Return u32str(type) || I || u32str(q) || K
            // Allocate the array to accomodate all elements of the key
            var fullKey = new byte[lmOtsAttribute.NumericIdentifier.Length + privateKey.I.Length + privateKey.Q.Length + k.Length];

            // typeCode | 32 bits / 4 bytes | fullKey[0]...fullKey[3]
            Array.Copy(lmOtsAttribute.NumericIdentifier, 0, fullKey, 0, 4);

            // I | 128 bits / 16 bytes | fullKey[4]...fullKey[19]
            Array.Copy(privateKey.I, 0, fullKey, 4, 16);

            // q | 32 bits / 4 bytes | fullKey[20]...fullKey[23]
            Array.Copy(privateKey.Q, 0, fullKey, 20, 4);

            Array.Copy(k, 0, fullKey, 24, k.Length);

            return fullKey;
        }
    }
}
