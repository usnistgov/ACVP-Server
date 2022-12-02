using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
{
    public class OtherInfoFactory : IOtherInfoFactory
    {
        private readonly IEntropyProvider _entropyProvider;

        public OtherInfoFactory(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
        }

        public IOtherInfo GetInstance(
            string otherInfoPattern,
            int otherInfoLength,
            KeyAgreementRole thisPartyKeyAgreementRole,
            PartyOtherInfo thisPartyOtherInfo,
            PartyOtherInfo otherPartyOtherInfo
        )
        {
            return new OtherInfo(
                _entropyProvider,
                otherInfoPattern,
                otherInfoLength,
                thisPartyKeyAgreementRole,
                thisPartyOtherInfo,
                otherPartyOtherInfo
            );
        }
    }
}
