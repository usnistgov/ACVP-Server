using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Crypto.Common.KAS.Builders
{
    public interface IKasBuilderKdfKc<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();

        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            WithKeyConfirmationDirection(KeyConfirmationDirection value);

        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyConfirmationRole(KeyConfirmationRole value);

        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyLength(int value);

        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithMacParameters(MacParameters value);

        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithOtherInfoPattern(string value);
    }
}