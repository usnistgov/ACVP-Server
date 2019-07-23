using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.NoKC
{
    /// <summary>
    /// Interface used for creating mac data for use in a no key confirmation KAS scheme
    /// </summary>
    public interface INoKeyConfirmationMacDataCreator
    {
        /// <summary>
        /// Get the mac data based on the passed <see cref="INoKeyConfirmationParameters"/>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BitString GetMacData(INoKeyConfirmationParameters param);
    }
}