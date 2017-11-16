using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    /// <summary>
    /// Interface for Key Agreement Schemes
    /// </summary>
    public interface IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IScheme<SchemeParametersBase<TParameterSet, TScheme>, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Scheme { get; }

        void SetDomainParameters(TDomainParameters domainParameters);
        /// <summary>
        /// Gets the shared information needed by the other party to complete key agreement
        /// </summary>
        /// <returns></returns>
        TOtherPartySharedInfo ReturnPublicInfoThisParty();
        /// <summary>
        /// The result of the key agreement attempt
        /// </summary>
        /// <param name="otherPartySharedInformation">The other party's shared information</param>
        /// <returns></returns>
        KasResult ComputeResult(TOtherPartySharedInfo otherPartySharedInformation);
    }
    
}