using NIST.CVP.Crypto.KeyWrap.Enums;

namespace NIST.CVP.Crypto.KeyWrap
{
    /// <summary>
    /// Interface for retrieving an instance of a <see cref="IKeyWrap"/>
    /// </summary>
    public interface IKeyWrapFactory
    {
        /// <summary>
        /// Gets a <see cref="IKeyWrap"/> based on the <see cref="KeyWrapType"/>
        /// </summary>
        /// <param name="keyWrapType">The type that is used to determine which <see cref="IKeyWrap"/> to retrieve</param>
        /// <returns></returns>
        IKeyWrap GetKeyWrapInstance(KeyWrapType keyWrapType);
    }
}