namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders
{
    public interface IKasBuilder
    {
        /// <summary>
        /// Provides a scheme builder to the kas builder.
        /// </summary>
        /// <param name="value">The scheme builder.</param>
        /// <returns>this instance of the builder.</returns>
        IKasBuilder WithSchemeBuilder(ISchemeBuilder value);
        /// <summary>
        /// Build the Kas Ifc instance
        /// </summary>
        /// <returns>An instantiated Kas instance.</returns>
        IKas Build();
    }
}