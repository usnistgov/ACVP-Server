using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeKdfFactory_BadDkm : IKdfFactory
    {
        private readonly IShaFactory _shaFactory;

        public FakeKdfFactory_BadDkm(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            switch (kdfHashMode)
            {
                case KdfHashMode.Sha:
                    return new FakeKdfSha_BadDkm(sha);
                default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }
    }

    internal class FakeKdfSha_BadDkm : KdfSha
    {
        public FakeKdfSha_BadDkm(ISha sha) : base(sha)
        {
        }

        public override KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo)
        {
            var dkmResult = base.DeriveKey(z, keyDataLength, otherInfo);

            // Change the DKM prior to returning
            dkmResult.DerivedKey[0] += 2;

            return dkmResult;
        }
    }
}