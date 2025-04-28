using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XECDH
{
    public class XecdhFactory : IXecdhFactory
    {
        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();

        public IXecdh GetXecdh(Curve curve, IEntropyProvider entropyProvider)
        {
            switch (curve)
            {
                case Curve.Curve25519:
                    return new X25519(curve, curve25519P, curve25519G, curve25519Bits, curve25519a24, entropyProvider);
                case Curve.Curve448:
                    return new X448(curve, curve448P, curve448G, curve448Bits, curve448a24, entropyProvider);
            }

            throw new ArgumentOutOfRangeException(nameof(curve));
        }

        public IXecdh GetXecdh(Curve curve, EntropyProviderTypes entropyType = EntropyProviderTypes.Random) {
            return GetXecdh(curve, _entropyFactory.GetEntropyProvider(entropyType));
        }

        #region Curve25519
        private readonly BigInteger curve25519P = LoadValue("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffed");
        private readonly BigInteger curve25519G = LoadValue("09");
        private readonly int curve25519Bits = 255;
        private readonly int curve25519a24 = 121665;
        #endregion Curve25519

        #region Curve448
        private readonly BigInteger curve448P = LoadValue("fffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
        private readonly BigInteger curve448G = LoadValue("05");
        private readonly int curve448Bits = 448;
        private readonly int curve448a24 = 39081;
        #endregion Curve448

        private static BigInteger LoadValue(string hex)
        {
            // Prepend a "0" if the string isn't valid hex (even number of characters)
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }

            return new BitString(hex).ToPositiveBigInteger();
        }
    }
}
