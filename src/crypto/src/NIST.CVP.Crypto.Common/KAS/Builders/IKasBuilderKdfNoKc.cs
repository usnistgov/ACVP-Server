using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Crypto.Common.KAS.Builders
{
    public interface IKasBuilderKdfNoKc<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();

        IKasBuilderKdfNoKc<
            TKasDsaAlgoAttributes, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            WithKeyLength(int value);

        IKasBuilderKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithMacParameters(MacParameters value);

        IKasBuilderKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithOtherInfoPattern(string value);
    }
}