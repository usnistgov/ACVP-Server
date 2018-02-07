using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used by the validation process to put information from the answer, 
    /// prompt, and result files "back together"
    /// </summary>
    /// <typeparam name="TTestVectorSet">The test vector set type.</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestReconstitutor<out TTestVectorSet, out TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Merges the answer and prompt files back into a <see cref="TTestVectorSet"/>
        /// </summary>
        /// <param name="answerResponse">The answer file</param>
        /// <param name="promptResponse">The prompt file</param>
        /// <returns></returns>
        TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse, dynamic promptResponse);
        /// <summary>
        /// Retrieves <see cref="TTestCase"/>s from the <see cref="resultResponse"/>
        /// </summary>
        /// <param name="resultResponse">The dynamic representation of the IUT supplied results.</param>
        /// <returns></returns>
        IEnumerable<TTestCase> GetTestCasesFromResultResponse(dynamic resultResponse);
    }
}
