using NIST.CVP.Common;

namespace NIST.CVP.Generation.GenValApp.Models
{
    /// <summary>
    /// Information on any additional dependencies required by the gen/val
    /// </summary>
    public class AdditionalDependencies
    {
        /// <summary>
        /// The assembly to load
        /// </summary>
        public string DependencyDll { get; set; }

        /// <summary>
        /// Does the assembly contain injections that should be registered?  
        /// Such as from a <see cref="IRegisterInjections"/>
        /// </summary>
        public bool HasInjectionRegistration { get; set; } = false;
    }
}