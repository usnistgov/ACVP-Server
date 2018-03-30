using System.Collections.Generic;
using System.Dynamic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestVectorSet : ITestVectorSet
    {
        public string Algorithm { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; } = false;
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();
        public List<dynamic> AnswerProjection { get; } = null;
        public List<dynamic> PromptProjection { get; } = null;
        public List<dynamic> ResultProjection { get; } = null;
        public dynamic ToDynamic()
        {
            dynamic vectorSetObject = new ExpandoObject();

            return vectorSetObject;
        }
    }
}
