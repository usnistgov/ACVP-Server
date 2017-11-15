using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    public class Kas<TParameterSet, TScheme> : IKas<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        private readonly IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme> _scheme;

        public Kas(IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme> scheme)
        {
            _scheme = scheme;
        }

        public IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme> Scheme => _scheme;

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
