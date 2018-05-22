using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfFactory : IKdfFactory
    {
        private readonly IShaFactory _shaFactory;
        
        public KdfFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            switch (kdfHashMode)
            {
                    case KdfHashMode.Sha:
                        return new KdfSha(sha);
                    default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }
    }
}