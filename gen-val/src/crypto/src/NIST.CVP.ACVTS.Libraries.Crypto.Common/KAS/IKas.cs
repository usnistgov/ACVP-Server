using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS
{
    /// <summary>
    /// Interface for Key Agreement Schemes
    /// </summary>
    public interface IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// The scheme used under the KAS instance
        /// </summary>
        IScheme<SchemeParametersBase<TKasDsaAlgoAttributes>, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Scheme { get; }

        /// <summary>
        /// Sets the domain parameters for the KAS instance
        /// </summary>
        /// <param name="domainParameters">The domain parameters to use.</param>
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
