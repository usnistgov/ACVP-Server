namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC
{
    /// <summary>
    /// Interface for retrieving an <see cref="IKeyConfirmation"/> instance
    /// </summary>
    public interface IKeyConfirmationFactory
    {
        IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters);
    }
}
