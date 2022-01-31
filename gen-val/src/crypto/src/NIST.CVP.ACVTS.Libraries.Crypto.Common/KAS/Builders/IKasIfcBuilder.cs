namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders
{
    /// <summary>
    /// Builder interface for getting an instance of Kas Ifc
    /// </summary>
    public interface IKasIfcBuilder
    {
        /// <summary>
        /// Provides a scheme builder to the kas builder.
        /// </summary>
        /// <param name="value">The scheme builder.</param>
        /// <returns>this instance of the builder.</returns>
        IKasIfcBuilder WithSchemeBuilder(ISchemeIfcBuilder value);
        /// <summary>
        /// Build the Kas Ifc instance
        /// </summary>
        /// <returns>An instantiated Kas instance.</returns>
        IKasIfc Build();
    }
}
