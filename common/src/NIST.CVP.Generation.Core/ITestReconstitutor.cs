using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used by the validation process to put information from the answer, 
    /// prompt, and result files "back together"
    /// </summary>
    /// <typeparam name="TTestVectorSet">The test vector set type.</typeparam>
    /// <typeparam name="TTestGroup">The test group type.</typeparam>
    public interface ITestReconstitutor<out TTestVectorSet, out TTestGroup>
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
    {
        /// <summary>
        /// Merges the answer and prompt files back into a <see cref="TTestVectorSet"/>
        /// </summary>
        /// <param name="answerResponse">The answer file</param>
        /// <returns></returns>
        TTestVectorSet GetTestVectorSetExpectationFromResponse(dynamic answerResponse);
        /// <summary>
        /// Retrieves <see cref="TTestGroup"/>s from the <see cref="resultResponse"/>
        /// </summary>
        /// <param name="resultResponse">The dynamic representation of the IUT supplied results.</param>
        /// <returns></returns>
        IEnumerable<TTestGroup> GetTestGroupsFromResultResponse(dynamic resultResponse);
    }
}
