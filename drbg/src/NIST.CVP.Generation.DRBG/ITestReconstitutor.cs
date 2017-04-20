using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public interface ITestReconstitutor<out TTestVectorSet, out TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse, dynamic promptResponse);
        IEnumerable<TTestCase> GetTestCasesFromResultResponse(dynamic resultResponse);
    }
}
