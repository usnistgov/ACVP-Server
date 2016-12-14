using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.Resolvers
{
    public class PromptResolver : ResolverBase
    {
        protected override string[] IgnoreProperties
        {
            get { return new[] { "answerprojection", "testresults" ,"issample" }; }
        }
    }
}
