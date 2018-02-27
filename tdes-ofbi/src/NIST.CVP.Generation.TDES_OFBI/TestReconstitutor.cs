using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFBI
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
            foreach (var resultGroup in resultResponse)
            {
                list.Add(new TestGroup(resultGroup));
            }

            return list;
        }
    }
}
