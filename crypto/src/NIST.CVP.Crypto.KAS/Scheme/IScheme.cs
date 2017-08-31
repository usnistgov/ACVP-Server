using NIST.CVP.Crypto.DSA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public interface IScheme
    {
        /// <summary>
        /// The static key pair used in the scheme (can be null)
        /// </summary>
        FfcDsaKeyPair StaticKeyPair { get; }
        /// <summary>
        /// The ephemeral key pair used in the scheme (can be null)
        /// </summary>
        FfcDsaKeyPair EphemeralKeyPair { get; }
        /// <summary>
        /// The Ephemeral public key nonce used in the scheme (can be null)
        /// </summary>
        BitString EphemeralNonce { get; }
        /// <summary>
        /// The DKM nonce used in the scheme (can be null)
        /// </summary>
        BitString DkmNonce { get; }

        /// <summary>
        /// Returns the shared information (public key/nonce information) 
        /// needed by the other party for KAS
        /// </summary>
        /// <returns></returns>
        FfcSharedInformation ReturnPublicInfoForOtherParty();
        /// <summary>
        /// Computes the KAS result based on the provided other party's shared information
        /// </summary>
        /// <param name="otherPartyInformation"></param>
        /// <returns></returns>
        KasResult ComputeResult(FfcSharedInformation otherPartyInformation);
    }
}