using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Crypto.KAS
{
    public class Kas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> : IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private readonly IScheme<SchemeParametersBase<TKasDsaAlgoAttributes>, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _scheme;

        public Kas(IScheme<SchemeParametersBase<TKasDsaAlgoAttributes>, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> scheme)
        {
            _scheme = scheme;
        }

        public IScheme<SchemeParametersBase<TKasDsaAlgoAttributes>, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Scheme => _scheme;

        public void SetDomainParameters(TDomainParameters domainParameters)
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
