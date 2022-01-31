using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes
{
    public class FakeKtsFactory_BadZ : IKtsFactory
    {
        private readonly IKtsFactory _ktsFactory;
        private readonly IRandom800_90 _random;

        public FakeKtsFactory_BadZ(IKtsFactory ktsFactory, IRandom800_90 random)
        {
            _ktsFactory = ktsFactory;
            _random = random;
        }

        public IRsaOaep Get(KasHashAlg hashAlg)
        {
            var kts = _ktsFactory.Get(hashAlg);

            return new FakeKts_BadZ(kts, _random);
        }
    }

    internal class FakeKts_BadZ : IRsaOaep
    {
        private readonly IRsaOaep _kts;
        private readonly IRandom800_90 _random;

        public FakeKts_BadZ(IRsaOaep kts, IRandom800_90 random)
        {
            _kts = kts;
            _random = random;
        }

        public SharedSecretResponse Encrypt(PublicKey rsaPublicKey, BitString keyingMaterial, BitString additionalInput)
        {
            var keyingMaterialByteLen = keyingMaterial.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var newKeyingMaterial = keyingMaterial.GetDeepCopy();

            // Modify a random byte in the keying material prior to executing the base method
            newKeyingMaterial[_random.GetRandomInt(0, keyingMaterialByteLen)] += 2;

            return _kts.Encrypt(rsaPublicKey, newKeyingMaterial, additionalInput);
        }

        public SharedSecretResponse Decrypt(KeyPair rsaKeyPair, BitString ciphertext, BitString additionalInput)
        {
            return _kts.Decrypt(rsaKeyPair, ciphertext, additionalInput);
        }
    }
}
