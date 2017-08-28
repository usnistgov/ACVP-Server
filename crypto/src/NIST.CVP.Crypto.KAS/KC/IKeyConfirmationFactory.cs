using NLog.LayoutRenderers.Wrappers;
using NLog.Targets;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <summary>
    /// Interface for retrieving an <see cref="IKeyConfirmation"/> instance
    /// </summary>
    public interface IKeyConfirmationFactory
    {
        IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters);
    }
}