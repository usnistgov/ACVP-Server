namespace NIST.CVP.Crypto.KAS
{
    /// <summary>
    /// Interface for retrieving an instance of <see cref="IKas"/>
    /// </summary>
    public interface IKasFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IKas"/> based on the parameters
        /// </summary>
        /// <param name="kasParameters">Parameters used to retrieve a Component only implementation of <see cref="IKas"/></param>
        /// <returns></returns>
        IKas GetInstance(KasParametersComponentOnly kasParameters);
        /// <summary>
        /// Gets an instance of <see cref="IKas"/> based on the parameters
        /// </summary>
        /// <param name="kasParameters">Parameters used to retrieve a No Key Confirmation implementation of <see cref="IKas"/></param>
        /// <returns></returns>
        IKas GetInstance(KasParametersNoKeyConfirmation kasParameters);
        /// <summary>
        /// Gets an instance of <see cref="IKas"/> based on the parameters
        /// </summary>
        /// <param name="kasParameters">Parameters used to retrieve a Key Confirmation implementation of <see cref="IKas"/></param>
        /// <returns></returns>
        IKas GetInstance(KasParametersKeyConfirmation kasParameters);
    }
}