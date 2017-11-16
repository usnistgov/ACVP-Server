using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfNoKc<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();

        IKasBuilderKdfNoKc<
            TParameterSet, 
            TScheme, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            WithKeyLength(int value);

        IKasBuilderKdfNoKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithMacParameters(MacParameters value);

        IKasBuilderKdfNoKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithOtherInfoPattern(string value);
    }
}