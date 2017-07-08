using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class TestReconstitutor : ITestReconstitutor<TestVectorSet, TestCase>
    {
        public TestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse, dynamic promptResponse)
        {
            return new TestVectorSet(answerResponse, promptResponse);
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
