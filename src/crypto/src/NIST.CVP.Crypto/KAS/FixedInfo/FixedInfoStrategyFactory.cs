using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfoStrategyFactory : IFixedInfoStrategyFactory
    {
        public IFixedInfoStrategy Get(KasKdfFixedInfoEncoding encoding)
        {
            switch (encoding)
            {
                case KasKdfFixedInfoEncoding.None:
                case KasKdfFixedInfoEncoding.Concatenation:
                    return new FixedInfoConcatenationStrategy();
                case KasKdfFixedInfoEncoding.ConcatenationWithLengths:
                    return new FixedInfoConcatenationLengthStrategy();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}