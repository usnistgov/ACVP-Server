using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    /// <summary>
    /// Describes methods for building an <see cref="IKas"/>
    /// </summary>
    public interface IKasBuilder
    {
        /// <summary>
        /// Returns a builder capaable of producing a <see cref="IKas"/> 
        /// with KDF and Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        KasBuilderKdfKc BuildKdfKc();
        /// <summary>
        /// Returns a builder capaable of producing a <see cref="IKas"/> 
        /// with KDF and without Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        KasBuilderKdfNoKc BuildKdfNoKc();
        /// <summary>
        /// Returns a builder capaable of producing a <see cref="IKas"/> 
        /// with no KDF and no Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        KasBuilderNoKdfNoKc BuildNoKdfNoKc();
        /// <summary>
        /// Sets the <see cref="KasAssurance"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        KasBuilder WithAssurances(KasAssurance value);
        /// <summary>
        /// Sets the <see cref="KeyAgreementRole"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        KasBuilder WithKeyAgreementRole(KeyAgreementRole value);
        /// <summary>
        /// Sets the <see cref="FfcParameterSet"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        KasBuilder WithParameterSet(FfcParameterSet value);
        /// <summary>
        /// Sets the PartyId for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        KasBuilder WithPartyId(BitString value);
        /// <summary>
        /// Sets the <see cref="FfcScheme"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        KasBuilder WithScheme(FfcScheme value);
    }
}