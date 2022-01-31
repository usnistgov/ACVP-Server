using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes
{
    public class FakeKtsFactory_BadDkm : IKtsFactory
    {
        private readonly IKtsFactory _ktsFactory;
        private readonly IRandom800_90 _random;

        public FakeKtsFactory_BadDkm(IKtsFactory ktsFactory, IRandom800_90 random)
        {
            _ktsFactory = ktsFactory;
            _random = random;
        }

        public IRsaOaep Get(KasHashAlg hashAlg)
        {
            var kts = _ktsFactory.Get(hashAlg);

            return new FakeKts_BadDkm(kts, _random);
        }
    }

    internal class FakeKts_BadDkm : IRsaOaep
    {
        private readonly IRsaOaep _kts;
        private readonly IRandom800_90 _random;

        public FakeKts_BadDkm(IRsaOaep kts, IRandom800_90 random)
        {
            _kts = kts;
            _random = random;
        }

        public SharedSecretResponse Encrypt(PublicKey rsaPublicKey, BitString keyingMaterial, BitString additionalInput)
        {
            var result = _kts.Encrypt(rsaPublicKey, keyingMaterial, additionalInput);

            var dkmByteLen = result.SharedSecretZ.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte in the result prior to returning
            result.SharedSecretZ[_random.GetRandomInt(0, dkmByteLen)] += 2;

            return result;
        }

        public SharedSecretResponse Decrypt(KeyPair rsaKeyPair, BitString ciphertext, BitString additionalInput)
        {
            return _kts.Decrypt(rsaKeyPair, ciphertext, additionalInput);
        }
    }
}
