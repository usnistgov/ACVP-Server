using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XECDH
{
    public class X25519 : XecdhBase
    {
        public X25519(Curve curveName, BigInteger p, BigInteger g, int bits, int a24, IEntropyProvider entropyProvider) :
            base(curveName, p, g, bits, a24, entropyProvider)
        {
        }

        public override XecdhKeyPairGenerateResult DeriveKeyPair(BitString privateKey)
        {
            // Section 6.1 of RFC 7748.
            if (privateKey.BitLength != 8 * 32)
            {
                return new XecdhKeyPairGenerateResult("Private key must be 32 bytes long");
            }

            var publicKey = XECDH(privateKey, EncodeUCoordinate(BasePointG));

            return new XecdhKeyPairGenerateResult(new XecdhKeyPair(publicKey, privateKey));
        }

        public override XecdhKeyPairGenerateResult GenerateKeyPair()
        {
            // Section 6.2 of RFC 7748.
            var privateKey = _entropyProvider.GetEntropy(8 * 32);

            return DeriveKeyPair(privateKey);
        }

        public override XecdhKeyPairValidateResult ValidateKeyPair(XecdhKeyPair keyPair)
        {
            // Section 6.1 of RFC 7748.
            if (keyPair.PublicKey.BitLength != 8 * 32)
            {
                return new XecdhKeyPairValidateResult("Public key must be 32 bytes long");
            }

            var result = DeriveKeyPair(keyPair?.PrivateKey);
            if (!result.Success)
            {
                return new XecdhKeyPairValidateResult(result.ErrorMessage);
            }

            // TODO: do we want to check pair-wise consistency? Currently not done for EdDSA.

            return new XecdhKeyPairValidateResult();
        }

        protected override BigInteger DecodeScalar(BitString k)
        {
            // Note that we don't care about how BitString is internally represented.
            var k_list = k.ToBytes();
            k_list[0] &= 248;
            k_list[31] &= 127;
            k_list[31] |= 64;
            return DecodeLittleEndian(k_list);
        }
    }
}
