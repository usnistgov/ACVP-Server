namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC
{
    /// <summary>
    /// Interface for retrieving an instance of <see cref="INoKeyConfirmationFactory"/>
    /// </summary>
    public interface INoKeyConfirmationFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="INoKeyConfirmation"/> based on the <see cref="INoKeyConfirmationParameters"/>
        /// </summary>
        /// <param name="parameters">The parameters used to determine what type of <see cref="INoKeyConfirmation"/> to return</param>
        /// <returns></returns>
        INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters);
    }
}
