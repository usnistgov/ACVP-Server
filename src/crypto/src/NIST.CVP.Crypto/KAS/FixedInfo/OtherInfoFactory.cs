using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.FixedInfo
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
