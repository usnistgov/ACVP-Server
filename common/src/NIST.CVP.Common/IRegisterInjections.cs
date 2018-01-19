using Autofac;

namespace NIST.CVP.Common
{
    /// <summary>
    /// Used to register concretions to the <see cref="ContainerBuilder"/> 
    /// and in turn <see cref="IContainer"/>.
    /// </summary>
    public interface IRegisterInjections
    {
        /// <summary>
        /// Register types onto the provided <see cref="ContainerBuilder"/>
        /// </summary>
        /// <param name="builder">The builder to register injectable components onto</param>
        void RegisterTypes(ContainerBuilder builder);
    }
}