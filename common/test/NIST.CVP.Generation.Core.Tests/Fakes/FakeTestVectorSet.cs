using System.Collections.Generic;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestVectorSet : ITestVectorSet
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; }
        public List<ITestGroup> TestGroups { get; set; }
        public List<dynamic> AnswerProjection { get; }
        public List<dynamic> PromptProjection { get; }
        public List<dynamic> ResultProjection { get; }

        public FakeTestVectorSet()
        {
            Algorithm = "FakeAlgo";
            IsSample = true;

            TestGroups = new List<ITestGroup>();
            TestGroups.Add(new FakeTestGroup());
        }
    }
}