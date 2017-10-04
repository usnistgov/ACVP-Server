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
        /// Sets the <see cref="KasAssurance"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder WithAssurances(KasAssurance value);
        /// <summary>
        /// Sets the <see cref="KeyAgreementRole"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder WithKeyAgreementRole(KeyAgreementRole value);
        /// <summary>
        /// Sets the <see cref="FfcParameterSet"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder WithParameterSet(FfcParameterSet value);
        /// <summary>
        /// Sets the PartyId for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder WithPartyId(BitString value);
        /// <summary>
        /// Sets the <see cref="FfcScheme"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder WithScheme(FfcScheme value);
        /// <summary>
        /// Sets the <see cref="ISchemeBuilder"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="schemeBuilder">The scheme builder to use</param>
        /// <returns></returns>
        IKasBuilder WithSchemeBuilder(ISchemeBuilder schemeBuilder);
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
    }
}