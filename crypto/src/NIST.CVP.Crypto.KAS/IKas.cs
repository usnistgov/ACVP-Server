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
    public interface IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        IScheme<SchemeParametersBase<TKasDsaAlgoAttributes>, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Scheme { get; }

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