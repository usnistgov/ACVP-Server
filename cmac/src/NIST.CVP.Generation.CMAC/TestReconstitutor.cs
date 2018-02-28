using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.CMAC
{
    public class TestReconstitutor<TTestVectorSet, TTestGroup> : ITestReconstitutor<TTestVectorSet, TTestGroup>
        where TTestVectorSet : TestVectorSetBase<TTestGroup>
        where TTestGroup : TestGroupBase, new()
    {
        public TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse)
        {
            return (TTestVectorSet)Activator.CreateInstance(typeof(TTestVectorSet), answerResponse);
        }

        public IEnumerable<TTestGroup> GetTestGroupsFromResultResponse(dynamic resultResponse)
        {
            var list = new List<TTestGroup>();
            foreach (var result in resultResponse)
            {
                list.Add((TTestGroup)Activator.CreateInstance(typeof(TTestGroup), result));
            }
            return list;
        }
    }
}
