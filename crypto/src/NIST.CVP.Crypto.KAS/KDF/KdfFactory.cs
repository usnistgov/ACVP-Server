using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;

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