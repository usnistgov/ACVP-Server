using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
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
            foreach(var result in resultResponse)
            {
                list.Add(new TestGroup(result));
            }

            return list;
        }
    }
}
