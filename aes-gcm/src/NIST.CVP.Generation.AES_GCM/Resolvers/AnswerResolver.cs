using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Resolvers
{
    public class AnswerResolver: ResolverBase
    {
        public static readonly AnswerResolver Instance = new AnswerResolver();

        protected override string[] IgnoreProperties
        {
            get { return new[] {"testgroups", "testresults"}; }
        }
    }
}
