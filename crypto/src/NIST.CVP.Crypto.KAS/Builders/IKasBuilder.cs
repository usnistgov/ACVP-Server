using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    /// <summary>
    /// Describes methods for building an <see cref="IKas"/>
    /// </summary>
    public interface IKasBuilder<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        /// <summary>
        /// Sets the <see cref="KasAssurance"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithAssurances(KasAssurance value);
        /// <summary>
        /// Sets the <see cref="KeyAgreementRole"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithKeyAgreementRole(KeyAgreementRole value);
        /// <summary>
        /// Sets the ParameterSet for the Kas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithParameterSet(TParameterSet value);
        /// <summary>
        /// Sets the PartyId for the Kas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithPartyId(BitString value);
        /// <summary>
        /// Sets the Scheme for the Kas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithScheme(TScheme value);
        /// <summary>
        /// Sets the scheme for the kas
        /// </summary>
        /// <param name="schemeBuilder">The scheme builder to use</param>
        /// <returns></returns>
        IKasBuilder<TParameterSet, TScheme> WithSchemeBuilder(ISchemeBuilder<TParameterSet, TScheme> schemeBuilder);
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with KDF and Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderKdfKc<TParameterSet, TScheme> BuildKdfKc();
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with KDF and without Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderKdfNoKc<TParameterSet, TScheme> BuildKdfNoKc();
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with no KDF and no Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderNoKdfNoKc<TParameterSet, TScheme> BuildNoKdfNoKc();
    }
}