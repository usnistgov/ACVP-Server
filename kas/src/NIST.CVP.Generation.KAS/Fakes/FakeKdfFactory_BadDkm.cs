using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeKdfFactory_BadDkm : IKdfFactory
    {
        private readonly IKdfFactory _kdfFactory;

        public FakeKdfFactory_BadDkm(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
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
    }

    internal class FakeKdfSha_BadDkm : IKdf
    {
        private readonly IKdf _kdf;

        public FakeKdfSha_BadDkm(IKdf kdf)
        {
            _kdf = kdf;
        }

        public KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo)
        {
            var dkmResult = _kdf.DeriveKey(z, keyDataLength, otherInfo);

            // Change the DKM prior to returning
            dkmResult.DerivedKey[0] += 2;

            return dkmResult;
        }
    }
}