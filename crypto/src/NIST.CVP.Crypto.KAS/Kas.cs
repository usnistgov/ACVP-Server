using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    public class Kas : IKas
    {
        private readonly IScheme _scheme;

        public Kas(IScheme scheme)
        {
            _scheme = scheme;
        }

        public SchemeParameters SchemeParameters => _scheme.SchemeParameters;

        public void SetDomainParameters(FfcDomainParameters domainParameters)
        {
            _scheme.SetDomainParameters(domainParameters);
        }

        public FfcSharedInformation ReturnPublicInfoThisParty()
        {
            return _scheme.ReturnPublicInfoThisParty();
        }

        public KasResult ComputeResult(FfcSharedInformation otherPartySharedInformation)
        {
            return _scheme.ComputeResult(otherPartySharedInformation);
        }
    }
}
