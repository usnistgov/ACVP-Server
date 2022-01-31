using System;
using System.Linq;

namespace NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            var interfaces = type.GetInterfaces();
            return interfaces.Any(t => t == interfaceType);
        }
    }
}
