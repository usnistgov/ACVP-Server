using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.CMAC
{
    public class TestReconstitutor<TTestVectorSet, TTestGroup, TTestCase> : ITestReconstitutor<TTestVectorSet, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        public TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse)
        {
            return (TTestVectorSet)Activator.CreateInstance(typeof(TTestVectorSet), answerResponse);
        }

        public IEnumerable<TTestCase> GetTestCasesFromResultResponse(dynamic resultResponse)
        {
            var list = new List<TTestCase>();
            foreach (var result in resultResponse)
            {
                list.Add((TTestCase)Activator.CreateInstance(typeof(TTestCase), result));
            }
            return list;
        }
    }
}
