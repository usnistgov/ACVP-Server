using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfKc<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();

        IKasBuilderKdfKc<
            TParameterSet, 
            TScheme, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            WithKeyConfirmationDirection(KeyConfirmationDirection value);

        IKasBuilderKdfKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyConfirmationRole(KeyConfirmationRole value);

        IKasBuilderKdfKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyLength(int value);

        IKasBuilderKdfKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithMacParameters(MacParameters value);

        IKasBuilderKdfKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithOtherInfoPattern(string value);
    }
}