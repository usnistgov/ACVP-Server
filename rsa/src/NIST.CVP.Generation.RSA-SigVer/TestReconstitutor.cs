using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
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
