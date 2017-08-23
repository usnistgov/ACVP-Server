namespace NIST.CVP.Crypto.KAS.KC
{
    /// <summary>
    /// Interface for describing Key Confirmation
    /// 
    /// U = key agreement initiator
    /// V = key agreement responder
    /// </summary>
    /// <typeparam name="TKeyConfirmationParameters"></typeparam>
    public interface IKeyConfirmation<in TKeyConfirmationParameters>
        where TKeyConfirmationParameters : IKeyConfirmationParameters
    {
        /// <summary>
        /// Computes a MAC based on the <see cref="keyConfirmationParameters"/>
        /// </summary>
        /// <param name="keyConfirmationParameters"></param>
        /// <returns></returns>
        ComputeKeyResult ComputeKeyMac(TKeyConfirmationParameters keyConfirmationParameters);
    }
}