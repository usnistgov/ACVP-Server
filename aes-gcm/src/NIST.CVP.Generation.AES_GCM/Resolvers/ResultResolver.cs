using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Resolvers
{
    public class ResultResolver : ResolverBase
    {
        protected override string[] IgnoreProperties
        {
            get { return new[] {"testgroups", "answerprojection"}; }
        }
    }
}
