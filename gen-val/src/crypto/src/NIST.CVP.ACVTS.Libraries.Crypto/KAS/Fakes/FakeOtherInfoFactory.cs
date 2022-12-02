using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes
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
