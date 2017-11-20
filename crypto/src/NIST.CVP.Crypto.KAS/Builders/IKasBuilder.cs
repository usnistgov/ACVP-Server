using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    /// <summary>
    /// Describes methods for building an <see cref="IKas"/>
    /// </summary>
    public interface IKasBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Sets the <see cref="KasAssurance"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<
            TKasDsaAlgoAttributes, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            WithAssurances(KasAssurance value);
        /// <summary>
        /// Sets the <see cref="KeyAgreementRole"/> for the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyAgreementRole(KeyAgreementRole value);
        /// <summary>
        /// Sets the ParameterSet for the Kas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithKasDsaAlgoAttributes(TKasDsaAlgoAttributes value);
        /// <summary>
        /// Sets the PartyId for the Kas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithPartyId(BitString value);
        /// <summary>
        /// Sets the scheme for the kas
        /// </summary>
        /// <param name="schemeBuilder">The scheme builder to use</param>
        /// <returns></returns>
        IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithSchemeBuilder(
                ISchemeBuilder<
                    TKasDsaAlgoAttributes, 
                    TOtherPartySharedInfo, 
                    TDomainParameters, 
                    TKeyPair
                > schemeBuilder
            );
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with KDF and Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            BuildKdfKc();
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with KDF and without Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            BuildKdfNoKc();
        /// <summary>
        /// Returns a builder capable of producing a Kas
        /// with no KDF and no Key Confirmation capabilities.
        /// </summary>
        /// <returns></returns>
        IKasBuilderNoKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            BuildNoKdfNoKc();
    }
}