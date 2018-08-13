using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Fakes
{
    public class FakeKdfFactory_BadZ : IKdfFactory
    {
        private readonly IKdfFactory _kdfFactory;

        public FakeKdfFactory_BadZ(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var kdf = _kdfFactory.GetInstance(kdfHashMode, hashFunction);

            switch (kdfHashMode)
            {
                case KdfHashMode.Sha:
                    return new FakeKdfSha_BadZ(kdf);
                default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }
    }

    internal class FakeKdfSha_BadZ : IKdf
    {
        private readonly IKdf _kdf;

        public FakeKdfSha_BadZ(IKdf kdf)
        {
            _kdf = kdf;
        }

        public KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo)
        {
            // change the z prior to passing to the KDF
            z[0] += 2;

            return _kdf.DeriveKey(z, keyDataLength, otherInfo);
        }
    }
}