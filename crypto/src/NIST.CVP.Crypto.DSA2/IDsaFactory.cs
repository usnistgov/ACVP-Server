using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// Interface for obtaining a <see cref="IDsa"/> with all dependencies.
    /// </summary>
    public interface IDsaFactory
    {
        /// <summary>
        /// Returns a <see cref="IDsa"/> as a concrete type.
        /// </summary>
        /// <typeparam name="TDsa">The type to construct.</typeparam>
        /// <param name="hashFunction">The hash information to be used.</param>
        /// <returns></returns>
        TDsa GetDsaInstance<TDsa>(HashFunction hashFunction) 
            where TDsa : class, IDsa;
    }
}