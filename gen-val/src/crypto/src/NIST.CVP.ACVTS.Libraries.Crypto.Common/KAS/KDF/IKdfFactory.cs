namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Interface for retrieving an instance of a <see cref="IKdf"/>.
    /// </summary>
    public interface IKdfFactory
    {
        /// <summary>
        /// Gets an instance of a <see cref="IKdf"/>.
        /// </summary>
        /// <returns>The constructed <see cref="IKdf"/>.</returns>
        IKdf GetKdf();
    }
}
