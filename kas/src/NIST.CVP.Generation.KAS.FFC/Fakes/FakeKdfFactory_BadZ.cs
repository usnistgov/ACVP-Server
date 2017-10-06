using System;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Fakes
{
    public class FakeKdfFactory_BadZ : IKdfFactory
    {
        private readonly IShaFactory _shaFactory;

        public FakeKdfFactory_BadZ(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            switch (kdfHashMode)
            {
                case KdfHashMode.Sha:
                    return new FakeKdfSha_BadZ(sha);
                default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }
    }

    public class FakeKdfSha_BadZ : KdfSha
    {
        public FakeKdfSha_BadZ(ISha sha) : base(sha)
        {
        }

        public override KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo)
        {
            // change the z prior to passing to the KDF
            z[0] += 2;

            return base.DeriveKey(z, keyDataLength, otherInfo);
        }
    }
}