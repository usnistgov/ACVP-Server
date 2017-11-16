using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class OtherInfoFactory<TSharedInformation, TDomainParameters, TKeyPair> : IOtherInfoFactory<TSharedInformation, TDomainParameters, TKeyPair>
        where TSharedInformation : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
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
            TSharedInformation thisPartySharedInformation,
            TSharedInformation otherPartySharedInformation
        )
        {
            return new OtherInfo<TSharedInformation, TDomainParameters, TKeyPair>(
                _entropyProvider, 
                otherInfoPattern, 
                otherInfoLength, 
                thisPartyKeyAgreementRole, 
                thisPartySharedInformation, 
                otherPartySharedInformation
            );
        }
    }
}
