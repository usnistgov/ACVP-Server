using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// A collection of tests for a crypto algorithm.
    /// </summary>
    public interface ITestVectorSet
    {
        /// <summary>
        /// The algorithm being tested
        /// </summary>
        string Algorithm { get; set; }
        /// <summary>
        /// The algorithm mode.
        /// </summary>
        string Mode { get; set; }
        /// <summary>
        /// Is this a sample vector set?
        /// </summary>
        bool IsSample { get; set; }
        /// <summary>
        /// The test groups associated with the vector set
        /// </summary>
        List<ITestGroup> TestGroups { get; set; }
        /// <summary>
        /// The answer projection - contains the crypto operation's "answers"
        /// Retained on the server.
        /// </summary>
        List<dynamic> AnswerProjection { get; }
        /// <summary>
        /// The prompt projection - contains all necessary inputs the IUT 
        /// should plug into their crypto implementation.
        /// </summary>
        List<dynamic> PromptProjection { get; }
        /// <summary>
        /// The result projection - contains the expected answers (and file format) 
        /// that should be received back from the IUT.  Does not include answers for "deferred" tests,
        /// unless isSample.
        /// Retained on the server.
        /// </summary>
        List<dynamic> ResultProjection { get; }

    }
}
