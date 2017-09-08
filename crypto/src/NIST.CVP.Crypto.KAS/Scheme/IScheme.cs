using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public interface IScheme
    {
        /// <summary>
        /// The length of the OtherInfo that is plugged into the KDF key.
        /// </summary>
        int OtherInputLength { get; }
        /// <summary>
        /// The Scheme utilized in the KAS negotiation
        /// </summary>
        FfcScheme Scheme { get; }
        /// <summary>
        /// The domain parameters associated with key generation
        /// </summary>
        FfcDomainParameters DomainParameters { get; }
        /// <summary>
        /// The static key pair used in the scheme (can be null)
        /// </summary>
        FfcKeyPair StaticKeyPair { get; }
        /// <summary>
        /// The ephemeral key pair used in the scheme (can be null)
        /// </summary>
        FfcKeyPair EphemeralKeyPair { get; }
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
        /// Sets the domain parameters for use in <see cref="IDsaFfc"/>
        /// </summary>
        /// <param name="domainParameters">The domain parameters to set</param>
        void SetDomainParameters(FfcDomainParameters domainParameters);

        /// <summary>
        /// Returns the shared information (public key/nonce information) 
        /// needed by the other party for KAS
        /// </summary>
        /// <returns></returns>
        FfcSharedInformation ReturnPublicInfoThisParty();
        /// <summary>
        /// Computes the KAS result based on the provided other party's shared information
        /// </summary>
        /// <param name="otherPartyInformation"></param>
        /// <returns></returns>
        KasResult ComputeResult(FfcSharedInformation otherPartyInformation);
    }
}