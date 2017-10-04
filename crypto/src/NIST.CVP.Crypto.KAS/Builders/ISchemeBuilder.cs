using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Builders
{
    /// <summary>
    /// Interface for building a scheme instance
    /// </summary>
    public interface ISchemeBuilder
    {
        /// <summary>
        /// Sets the <see cref="IDsaFfc"/> used in the scheme.
        /// </summary>
        /// <param name="dsa">The dsa implementation to use</param>
        /// <returns></returns>
        ISchemeBuilder WithDsa(IDsaFfc dsa);
        
        /// <summary>
        /// Sets the <see cref="IOtherInfoFactory"/> used in the scheme
        /// </summary>
        /// <param name="otherInfoFactory">The other info factory used in the scheme.</param>
        /// <returns></returns>
        ISchemeBuilder WithOtherInfoFactory(IOtherInfoFactory otherInfoFactory);

        /// <summary>
        /// Sets the <see cref="IEntropyProvider"/> used in the scheme.
        /// </summary>
        /// <param name="entropyProvider">The entropy provider used in the scheme.</param>
        /// <returns></returns>
        ISchemeBuilder WIthEntropyProvider(IEntropyProvider entropyProvider);

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
        IScheme BuildScheme(SchemeParameters schemeParameters, KdfParameters kdfParameters,
            MacParameters macParameters, bool backToOriginalState = true);
    }
}