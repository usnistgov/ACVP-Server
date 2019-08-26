using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    public class FakeKdfFactory_BadDkm : IKdfOneStepFactory
    {
        private readonly IKdfOneStepFactory _kdfFactory;

        public FakeKdfFactory_BadDkm(IKdfOneStepFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdfOneStep GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var kdf = _kdfFactory.GetInstance(kdfHashMode, hashFunction);

            switch (kdfHashMode)
            {
                case KdfHashMode.Sha:
                    return new FakeKdfSha_BadDkm(kdf);
                default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }

        public IKdfOneStep GetInstance(OneStepConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeKdfSha_BadDkm : IKdfOneStep
    {
        private readonly IKdfOneStep _kdf;

        public FakeKdfSha_BadDkm(IKdfOneStep kdf)
        {
            _kdf = kdf;
        }

        public KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo, BitString salt)
        {
            var dkmResult = _kdf.DeriveKey(z, keyDataLength, otherInfo, salt);

            // Change the DKM prior to returning
            dkmResult.DerivedKey[0] += 2;

            return dkmResult;
        }
    }
}