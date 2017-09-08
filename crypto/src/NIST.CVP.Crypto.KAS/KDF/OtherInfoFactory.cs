using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.KDF
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
            FfcSharedInformation thisPartySharedInformation, 
            FfcSharedInformation otherPartySharedInformation
        )
        {
            return new OtherInfo(_entropyProvider, otherInfoPattern, otherInfoLength, thisPartyKeyAgreementRole, thisPartySharedInformation, otherPartySharedInformation);
        }
    }
}
