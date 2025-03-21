using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XECDH
{
    public abstract class XecdhBase : IXecdh
    {

        public Curve CurveName { get; }

        public BigInteger FieldSizeP { get; }

        public BigInteger BasePointG { get; }

        public int VariableBits { get; }

        public int VariableA24 { get; }

        protected readonly IEntropyProvider _entropyProvider;

        protected readonly PrimeFieldOperator _operator;

        public XecdhBase(Curve curveName, BigInteger p, BigInteger g, int bits, int a24, IEntropyProvider entropyProvider)
        {
            CurveName = curveName;
            FieldSizeP = p;
            BasePointG = g;
            VariableBits = bits;
            VariableA24 = a24;

            _entropyProvider = entropyProvider;
            _operator = new PrimeFieldOperator(p);
        }

        public BitString XECDH(BitString k, BitString u)
        {
            return EncodeUCoordinate(XECDH(DecodeScalar(k), DecodeUCoordinate(u)));
        }

        public abstract XecdhKeyPairGenerateResult DeriveKeyPair(BitString privateKey);

        public abstract XecdhKeyPairGenerateResult GenerateKeyPair();

        public abstract XecdhKeyPairValidateResult ValidateKeyPair(XecdhKeyPair keyPair);

        protected abstract BigInteger DecodeScalar(BitString k);

        protected BigInteger DecodeLittleEndian(byte[] b)
        {
            BigInteger decoded = 0;
            for (var i = 0; i < ((VariableBits + 7) / 8); i++)
            {
                decoded += new BigInteger(b[i]) << (8 * i);
            }
            return decoded;
        }

        protected BigInteger DecodeUCoordinate(BitString u)
        {
            // Note that we don't care about how BitString is internally represented.
            var u_list = u.ToBytes();
            if ((VariableBits % 8) > 0)
            {
                u_list[^1] &= (byte)((1 << (VariableBits % 8)) - 1);
            }

            return DecodeLittleEndian(u_list);
        }

        protected BitString EncodeUCoordinate(BigInteger u)
        {
            u = _operator.Modulo(u);

            byte[] encoded = new byte[((VariableBits + 7) / 8)];
            for (var i = 0; i < ((VariableBits + 7) / 8); i++)
            {
                encoded[i] = (byte)((u >> 8 * i) & 0xff);
            }

            // Note that we don't care about how BitString is internally represented.
            return new BitString(encoded);
        }

        private BigInteger XECDH(BigInteger k, BigInteger u)
        {
            // Section 5 of IETF RFC 7748.
            BigInteger x_1 = u;
            BigInteger x_2 = 1;
            BigInteger z_2 = 0;
            BigInteger x_3 = u;
            BigInteger z_3 = 1;
            BigInteger tmp;
            var swap = 0;

            for (var t = VariableBits - 1; t >= 0; t--)
            {
                var k_t = (int)((k >> t) & 1);
                swap ^= k_t;

                // For simplicity, we do not implement the conditional swap.
                if (swap == 1)
                {
                    (x_2, x_3) = (x_3, x_2);
                    (z_2, z_3) = (z_3, z_2);
                }

                swap = k_t;

                var A = _operator.Add(x_2, z_2);
                var AA = _operator.Multiply(A, A);
                var B = _operator.Subtract(x_2, z_2);
                var BB = _operator.Multiply(B, B);
                var E = _operator.Subtract(AA, BB);
                var C = _operator.Add(x_3, z_3);
                var D = _operator.Subtract(x_3, z_3);
                var DA = _operator.Multiply(D, A);
                var CB = _operator.Multiply(C, B);
                tmp = _operator.Add(DA, CB);
                x_3 = _operator.Multiply(tmp, tmp);
                tmp = _operator.Subtract(DA, CB);
                z_3 = _operator.Multiply(x_1, _operator.Multiply(tmp, tmp));
                x_2 = _operator.Multiply(AA, BB);
                z_2 = _operator.Multiply(E, _operator.Add(AA, _operator.Multiply(VariableA24, E)));
            }

            // For simplicity, we do not implement the conditional swap.
            if (swap == 1)
            {
                (x_2, x_3) = (x_3, x_2);
                (z_2, z_3) = (z_3, z_2);
            }

            // By Fermat's Little Theorem, z_2^(p - 2) = z^-1 mod p.
            return _operator.Multiply(x_2, z_2.ModularInverse(FieldSizeP));
        }
    }
}