using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
{
    public class FixedInfoStrategyFactory : IFixedInfoStrategyFactory
    {
        public IFixedInfoStrategy Get(FixedInfoEncoding encoding)
        {
            switch (encoding)
            {
                case FixedInfoEncoding.None:
                case FixedInfoEncoding.Concatenation:
                    return new FixedInfoConcatenationStrategy();
                case FixedInfoEncoding.ConcatenationWithLengths:
                    return new FixedInfoConcatenationLengthStrategy();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
