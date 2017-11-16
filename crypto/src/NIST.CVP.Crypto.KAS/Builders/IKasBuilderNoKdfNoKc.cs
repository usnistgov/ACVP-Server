using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderNoKdfNoKc<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();
    }
}