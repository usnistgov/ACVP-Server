using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Used to describe the valid ranges, primitives, etc in use for a KDF as described in:
    ///
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Cr1.pdf
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-108.pdf
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-135r1.pdf
    /// </summary>
    public interface IKasKdfConfiguration
    {
        /// <summary>
        /// The enum type of KDF
        /// </summary>
        KasKdf KdfType { get; }
    }
}