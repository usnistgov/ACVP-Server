using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;

namespace NIST.CVP.Crypto.Common.MAC.CMAC
{
    /// <summary>
    /// Interface for retrieving an instance of <see cref="ICmac"/>
    /// </summary>
    public interface ICmacFactory
    {
        /// <summary>
        /// Retrieves a <see cref="ICmac"/> based on <see cref="cmacType"/>
        /// </summary>
        /// <param name="cmacType">The type that an instance should be retrieved based on.</param>
        /// <returns>The instance.</returns>
        ICmac GetCmacInstance(CmacTypes cmacType);
    }
}