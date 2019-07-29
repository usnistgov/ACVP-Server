using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KC
{
    /// <summary>
    /// Interface used for creating mac data for use in a key confirmation KAS scheme
    /// </summary>
    public interface IKeyConfirmationMacDataCreator
    {
        /// <summary>
        /// Get the mac data based on the passed <see cref="IKeyConfirmationParameters"/>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BitString GetMacData(IKeyConfirmationParameters param);
    }
}