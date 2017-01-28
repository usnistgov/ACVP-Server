using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES
{
    public interface ITestReconstitutor<out TTestVectorSet, out TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse, dynamic promptResponse);
        IEnumerable<TTestCase> GetTestCasesFromResultResponse(dynamic resultResponse);
    }
}
