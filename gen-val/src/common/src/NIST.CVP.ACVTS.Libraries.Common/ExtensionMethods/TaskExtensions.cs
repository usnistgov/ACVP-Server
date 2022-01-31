using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Useful for firing off tasks and not awaiting the result
        /// </summary>
        /// <param name="task"></param>
        public static void FireAndForget(this Task task) { }
    }
}
