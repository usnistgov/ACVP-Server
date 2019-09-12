using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Crypto.Common.KAS
{
    /// <summary>
    /// Root Interface for KAS utilizing integer factorization cryptography (rsa) .
    /// This is different enough from <see cref="IKas"/> operations that it warranted its own interface,
    /// at least for now.
    /// </summary>
    public interface IKasIfc
    {
        /// <summary>
        /// The KAS IFC Scheme utilized
        /// </summary>
        ISchemeIfc Scheme { get; }
        /// <summary>
        /// Initialize this party's secret keying material, based on another party's keying material (in some instances).
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties contribution to the KAS/KTS scheme.</param>
        void InitializeThisPartyKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial);
        /// <summary>
        /// Establish a key between this party and the other party.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties keying material that contributes to the key.</param>
        /// <returns>A <see cref="KasResult"/> that contains the result of the KAS attempt.</returns>
        KasIfcResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial);
    }
}