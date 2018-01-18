using Autofac;

namespace NIST.CVP.Common
{
    /// <summary>
    /// Used to register concretions to the <see cref="ContainerBuilder"/> 
    /// and in turn <see cref="IContainer"/>.
    /// </summary>
    public interface IRegisterInjections
    {
        void RegisterTypes(ContainerBuilder builder);
    }
}