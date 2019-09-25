using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.KAS.Builders
{
    /// <summary>
    /// Describes methods for building a Kas Ifc Scheme 
    /// </summary>
    public interface ISchemeIfcBuilder
    {
        /// <summary>
        /// This parties role in the kas scheme. 
        /// </summary>
        /// <param name="value">The parameters of this party as they relate to the scheme.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithSchemeParameters(SchemeParametersIfc value);
        /// <summary>
        /// Sets the full secret keying material for this party when available.
        /// </summary>
        /// <param name="value">The secret keying material for this party.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithThisPartyKeyingMaterial(IIfcSecretKeyingMaterial value);
        /// <summary>
        /// A builder used to construct this party's contribution to the secret keying material.
        /// </summary>
        /// <param name="value">Builder for constructing this party's secret keying material.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithThisPartyKeyingMaterialBuilder(IIfcSecretKeyingMaterialBuilder value);
        /// <summary>
        /// Provides a FixedInfo factory and parameter for constructing FixedInfo for use in a KDF or KTS scheme. 
        /// </summary>
        /// <param name="factory">The fixed info factory.</param>
        /// <param name="parameter">The fixed info parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithFixedInfo(IFixedInfoFactory factory, FixedInfoParameter parameter);
        /// <summary>
        /// Provides a KDF factory and parameter for deriving a key from a secret.
        /// </summary>
        /// <param name="factory">The KDF factory.</param>
        /// <param name="parameter">The KDF parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKdf(IKdfFactory factory, IKdfParameter parameter);
        /// <summary>
        /// Provides a KTS factory and parameter for wrapping/unwrapping a key.
        /// </summary>
        /// <param name="factory">The KTS factory.</param>
        /// <param name="parameter">The KTS parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKts(IKtsFactory factory, KtsParameter parameter);
        /// <summary>
        /// Provides a <see cref="IRsaSve"/> instance to the builder.
        /// </summary>
        /// <param name="value">The <see cref="IRsaSve"/> implementation.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithRsaSve(IRsaSve value);
        /// <summary>
        /// Provides a Key Confirmation factory and parameter for performing key confirmation..
        /// </summary>
        /// <param name="factory">The KC factory.</param>
        /// <param name="parameter">The KC parameters.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKeyConfirmation(IKeyConfirmationFactory factory, MacParameters parameter);

        /// <summary>
        /// Build the Kas Ifc Scheme with the specified parameters.
        /// </summary>
        /// <returns>An instance of <see cref="ISchemeIfc"/>.</returns>
        ISchemeIfcBuilder WithEntropyProvider(IEntropyProvider entropyProvider);
        ISchemeIfc Build();
    }
}