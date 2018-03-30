using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.MAC.HMAC
{
    /// <summary>
    /// Provides a means of retrieving a <see cref="IHmac"/>
    /// </summary>
    public interface IHmacFactory
    {
        /// <summary>
        /// Get an <see cref="IHmac"/> based on the <see cref="HashFunction"/>
        /// </summary>
        /// <param name="hashFunction">The hashfunction for retrieving an <see cref="ISha"/></param>
        /// <returns></returns>
        IHmac GetHmacInstance(HashFunction hashFunction);
    }
}