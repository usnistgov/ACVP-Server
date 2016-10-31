using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Resolvers
{
    public class ResultResolver: ResolverBase
    {
        public static readonly ResultResolver Instance = new ResultResolver();

        protected override string[] IgnoreProperties
        {
            get { return new[] {"PromptProjection", "AnswerProjection", "TestGroups"}; }
        }
    }
}
