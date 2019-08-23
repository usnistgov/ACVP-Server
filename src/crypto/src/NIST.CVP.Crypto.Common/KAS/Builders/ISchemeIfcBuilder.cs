using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;

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
        /// The other party's key contributions for the KAS operation.
        /// </summary>
        /// <param name="value">The other party's secreting keying material.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithOtherPartyKeyingMaterial(IIfcSecretKeyingMaterial value);
        /// <summary>
        /// Provides an instance of a KdfFactory to the builder. 
        /// </summary>
        /// <param name="value">The KDF factory to utilize.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKdfFactory(IKdfOneStepFactory value);
        /// <summary>
        /// Provides an instance of a KtsFactory to the builder.
        /// </summary>
        /// <param name="value">The KTS factory to utilize.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKtsFactory(IKtsFactory value);
        /// <summary>
        /// Provides an instance of a KeyConfirmation factory to utilize.
        /// </summary>
        /// <param name="value">The KeyConfirmation factory to utilize.</param>
        /// <returns>this builder.</returns>
        ISchemeIfcBuilder WithKeyConfirmationFactory(IKeyConfirmationFactory value);
        /// <summary>
        /// Build the Kas Ifc Scheme with the specified parameters.
        /// </summary>
        /// <returns>An instance of <see cref="ISchemeIfc"/>.</returns>
        ISchemeIfc Build();
    }
}