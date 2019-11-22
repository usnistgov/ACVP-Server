using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders
{
    /// <summary>
    /// Describes methods for building a Kas  Scheme 
    /// </summary>
    public interface ISchemeBuilder
    {
        /// <summary>
        /// This parties role in the kas scheme. 
        /// </summary>
        /// <param name="value">The parameters of this party as they relate to the scheme.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithSchemeParameters(SchemeParameters value);
        /// <summary>
        /// Sets the full secret keying material for this party when available.
        /// </summary>
        /// <param name="value">The secret keying material for this party.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithThisPartyKeyingMaterial(ISecretKeyingMaterial value);
        /// <summary>
        /// Provides a FixedInfo factory and parameter for constructing FixedInfo for use in a KDF or KTS scheme. 
        /// </summary>
        /// <param name="factory">The fixed info factory.</param>
        /// <param name="parameter">The fixed info parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithFixedInfo(IFixedInfoFactory factory, FixedInfoParameter parameter);
        /// <summary>
        /// Provides a KDF factory and parameter for deriving a key from a secret.
        /// </summary>
        /// <param name="factory">The KDF factory.</param>
        /// <param name="parameter">The KDF parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithKdf(IKdfFactory factory, IKdfParameter parameter);
        /// <summary>
        /// Provides a Key Confirmation factory and parameter for performing key confirmation.
        /// </summary>
        /// <param name="factory">The KC factory.</param>
        /// <param name="parameter">The KC parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithKeyConfirmation(IKeyConfirmationFactory factory, MacParameters parameter);
        /// <summary>
        /// Provides an entropy provider to the built instance.
        /// </summary>
        /// <param name="entropyProvider">The entropy provider.</param>
        /// <returns>this builder.</returns>
        ISchemeBuilder WithEntropyProvider(IEntropyProvider entropyProvider);
        /// <summary>
        /// Build the Kas Scheme with the specified parameters.
        /// </summary>
        /// <returns>An instance of <see cref="IScheme"/>.</returns>
        IScheme Build();
    }
}