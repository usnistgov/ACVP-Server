using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestReconstitutor : ITestReconstitutor<TestVectorSet, TestCase>
    {
        public TestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse)
        {
            return new TestVectorSet(answerResponse);
        }

        public IEnumerable<TestCase> GetTestCasesFromResultResponse(dynamic resultResponse)
        {
            var list = new List<TestCase>();
            foreach (var result in resultResponse)
            {
                list.Add(new TestCase(result));
            }
            return list;
        }
    }
}