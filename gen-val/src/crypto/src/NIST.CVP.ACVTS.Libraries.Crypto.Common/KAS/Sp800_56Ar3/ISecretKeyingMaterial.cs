using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3
{
    /// <summary>
    /// Represents the information that goes into the construction of a KAS parties secret keying material.
    /// </summary>
    public interface ISecretKeyingMaterial
    {
        /// <summary>
        /// The underlying algorithm utilized for the secret keying material and kas scheme.
        /// </summary>
        KasAlgorithm KasAlgorithm { get; }
        /// <summary>
        /// The domain parameters used to generate keys.
        /// </summary>
        IDsaDomainParameters DomainParameters { get; }
        /// <summary>
        /// The static keyPair belonging to the party.
        /// </summary>
        IDsaKeyPair StaticKeyPair { get; }
        /// <summary>
        /// The ephemeral keyPair belonging to the party.
        /// </summary>
        IDsaKeyPair EphemeralKeyPair { get; }
        /// <summary>
        /// Ephemeral nonce belonging to the party used for some KAS schemes.
        /// </summary>
        BitString EphemeralNonce { get; }
        /// <summary>
        /// The nonce used for the generation of the derived keying material in some KAS schemes..
        /// </summary>
        BitString DkmNonce { get; }
        /// <summary>
        /// The party identifier.
        /// </summary>
        BitString PartyId { get; }
    }
}
