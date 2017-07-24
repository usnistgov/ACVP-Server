using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestReconstitutorTdes : ITestReconstitutor<TestVectorSetTdes, TestCaseTdes>
    {
        public TestVectorSetTdes GetTestVectorSetExpectationFromResponse(dynamic answerResponse, dynamic promptResponse)
        {
            return new TestVectorSetTdes(answerResponse, promptResponse);
        }

        public IEnumerable<TestCaseTdes> GetTestCasesFromResultResponse(dynamic resultResponse)
        {
            var list = new List<TestCaseTdes>();
            foreach (var result in resultResponse)
            {
                list.Add(new TestCaseTdes(result));
            }
            return list;
        }
    }
}
