using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestReconstitutor : ITestReconstitutor<TestVectorSet, TestGroup>
    {
        public TestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse)
        {
            return new TestVectorSet(answerResponse);
        }

        public IEnumerable<TestGroup> GetTestGroupsFromResultResponse(dynamic resultResponse)
        {
            var list = new List<TestGroup>();
            foreach (var result in resultResponse)
            {
                list.Add(new TestGroup(result));
            }
            return list;
        }
    }
}
