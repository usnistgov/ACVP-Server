using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Fakes
{
    public class FakeOtherInfoFactory : IOtherInfoFactory
    {
        private readonly BitString _otherInfo;

        public FakeOtherInfoFactory(BitString otherInfo)
        {
            _otherInfo = otherInfo;
        }

        public IOtherInfo GetInstance(string otherInfoPattern, int otherInfoLength, KeyAgreementRole thisPartyKeyAgreementRole,
            FfcSharedInformation thisPartySharedInformation, FfcSharedInformation otherPartySharedInformation)
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