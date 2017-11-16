using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    public class Kas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> : IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private readonly IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _scheme;

        public Kas(IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> scheme)
        {
            _scheme = scheme;
        }

        public IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Scheme => _scheme;

        public void SetDomainParameters(FfcDomainParameters domainParameters)
        {
            _scheme.SetDomainParameters(domainParameters);
        }

        public TOtherPartySharedInfo ReturnPublicInfoThisParty()
        {
            return _scheme.ReturnPublicInfoThisParty();
        }

        public KasResult ComputeResult(TOtherPartySharedInfo otherPartySharedInformation)
        {
            return _scheme.ComputeResult(otherPartySharedInformation);
        }
    }
}
