using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeOtherInfoFactory : IOtherInfoFactory
    {
        private readonly BitString _otherInfo;

        public FakeOtherInfoFactory(BitString otherInfo)
        {
            _otherInfo = otherInfo;
        }

        public IOtherInfo GetInstance(
            string otherInfoPattern, 
            int otherInfoLength, 
            KeyAgreementRole thisPartyKeyAgreementRole,
            PartyOtherInfo thisPartySharedInformation,
            PartyOtherInfo otherPartySharedInformation)
        {
            return new FakeOtherInfo(_otherInfo);
        }
    }

    internal class FakeOtherInfo : IOtherInfo
    {
        private readonly BitString _otherInfo;

        public FakeOtherInfo(BitString otherInfo)
        {
            _otherInfo = otherInfo;
        }

        public BitString GetOtherInfo()
        {
            return _otherInfo;
        }
    }
}