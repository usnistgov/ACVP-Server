using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders
{
    /// <summary>
    /// Interface for building a scheme instance
    /// </summary>
    public interface ISchemeBuilder<TParameterSet, TScheme, TSharedInformation, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TSharedInformation : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Sets the <see cref="HashFunction" /> used for DSA.
        /// </summary>
        /// <param name="hashFunction">The hash function to use</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet, 
            TScheme, 
            TSharedInformation, 
            TDomainParameters, 
            TKeyPair
        > 
            WithHashFunction(HashFunction hashFunction);

        /// <summary>
        /// Sets the <see cref="IDsaFfcFactory"/> used in the scheme.
        /// </summary>
        /// <param name="dsaFactory">The dsa implementation to use</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet, 
            TScheme, 
            TSharedInformation, 
            TDomainParameters, 
            TKeyPair
        > 
            WithDsaFactory(IDsaFfcFactory dsaFactory);

        /// <summary>
        /// Sets the <see cref="IKdfFactory"/> used in the scheme.
        /// </summary>
        /// <param name="kdfFactory">The kdf factory to use</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet,
            TScheme,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithKdfFactory(IKdfFactory kdfFactory);


        /// <summary>
        /// Sets the <see cref="IKeyConfirmationFactory"/> used in the scheme.
        /// </summary>
        /// <param name="keyConfirmationFactory">The key confirmation factory implementation to use</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet,
            TScheme,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithKeyConfirmationFactory(IKeyConfirmationFactory keyConfirmationFactory);

        /// <summary>
        /// Sets the <see cref="IKeyConfirmationFactory"/> used in the scheme.
        /// </summary>
        /// <param name="noKeyConfirmationFactory">The kdf factory implementation to use</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet,
            TScheme,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithNoKeyConfirmationFactory(INoKeyConfirmationFactory noKeyConfirmationFactory);


        /// <summary>
        /// Sets the OtherInfoFactory used in the scheme
        /// </summary>
        /// <param name="otherInfoFactory">The other info factory used in the scheme.</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet,
            TScheme,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithOtherInfoFactory(IOtherInfoFactory<TSharedInformation, TDomainParameters, TKeyPair> otherInfoFactory);

        /// <summary>
        /// Sets the <see cref="IEntropyProvider"/> used in the scheme.
        /// </summary>
        /// <param name="entropyProvider">The entropy provider used in the scheme.</param>
        /// <returns></returns>
        ISchemeBuilder<
            TParameterSet,
            TScheme,
            TSharedInformation,
            TDomainParameters,
            TKeyPair
        > 
            WithEntropyProvider(IEntropyProvider entropyProvider);
        
        /// <summary>
        /// Builds the scheme using the provided parameters, and default (or overriden dependencies for testing)
        /// </summary>
        /// <param name="schemeParameters">The scheme parameters</param>
        /// <param name="kdfParameters">KDF parameters (can be null)</param>
        /// <param name="macParameters">MAC parameters (can be null)</param>
        /// <param name="backToOriginalState">
        ///     Sets the builder back to the original state, utilizing the dependencies from construction, 
        /// rather any overriden after construction.
        /// </param>
        /// <returns></returns>
        IScheme<
            SchemeParametersBase<
                TParameterSet, 
                TScheme
            >, 
            TParameterSet, 
            TScheme, 
            TSharedInformation, 
            TDomainParameters, 
            TKeyPair
        >
            BuildScheme(
                SchemeParametersBase<
                    TParameterSet, 
                    TScheme
                > schemeParameters, 
                KdfParameters kdfParameters,
                MacParameters macParameters, 
                bool backToOriginalState = true
            );
    }
}