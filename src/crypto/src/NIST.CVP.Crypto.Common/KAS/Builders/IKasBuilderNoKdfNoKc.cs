using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Crypto.Common.KAS.Builders
{
    public interface IKasBuilderNoKdfNoKc<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();
    }
}