using NIST.CVP.Crypto.Common.KAS.FixedInfo;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfoFactory : IFixedInfoFactory
    {
        public IFixedInfo Get()
        {
            return new FixedInfo();
        }
    }
}