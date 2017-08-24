using NLog.LayoutRenderers.Wrappers;
using NLog.Targets;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <summary>
    /// Interface for retrieving an <see cref="IKeyConfirmation{TKeyConfirmationParameters}"/> instance
    /// </summary>
    public interface IKeyConfirmationFactory
    {
        object GetInstance<TKeyConfirmationParameters>(TKeyConfirmationParameters parameters)
            where TKeyConfirmationParameters : IKeyConfirmationParameters;
    }
}