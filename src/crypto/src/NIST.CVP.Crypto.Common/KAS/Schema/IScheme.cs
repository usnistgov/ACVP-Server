using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Schema
{
    public interface IScheme<out TSchemeParameters, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TSchemeParameters : ISchemeParameters<TKasDsaAlgoAttributes>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// The length of the OtherInfo that is plugged into the KDF key.
        /// </summary>
        int OtherInputLength { get; }
        /// <summary>
        /// The Scheme parameters for this party
        /// </summary>
        TSchemeParameters SchemeParameters { get; }
        /// <summary>
        /// The domain parameters associated with key generation
        /// </summary>
        TDomainParameters DomainParameters { get; }
        /// <summary>
        /// The static key pair used in the scheme (can be null)
        /// </summary>
        TKeyPair StaticKeyPair { get; }
        /// <summary>
        /// The ephemeral key pair used in the scheme (can be null)
        /// </summary>
        TKeyPair EphemeralKeyPair { get; }
        /// <summary>
        /// The Ephemeral public key nonce used in the scheme (can be null)
        /// </summary>
        BitString EphemeralNonce { get; }
        /// <summary>
        /// The DKM nonce used in the scheme (can be null)
        /// </summary>
        BitString DkmNonce { get; }
        /// <summary>
        /// The nonce used in a no key confirmation scenario
        /// </summary>
        BitString NoKeyConfirmationNonce { get; }

        /// <summary>
        /// Sets the domain parameters for use in DSA
        /// </summary>
        /// <param name="domainParameters">The domain parameters to set</param>
        void SetDomainParameters(TDomainParameters domainParameters);

        /// <summary>
        /// Returns the shared information (public key/nonce information) 
        /// needed by the other party for KAS
        /// </summary>
        /// <returns></returns>
        TOtherPartySharedInfo ReturnPublicInfoThisParty();
        
        /// <summary>
        /// Computes the KAS result based on the provided other party's shared information
        /// </summary>
        /// <param name="otherPartyInformation"></param>
        /// <returns></returns>
        KasResult ComputeResult(TOtherPartySharedInfo otherPartyInformation);
    }
}