﻿using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Async
{
    /// <summary>
    /// Provides a means of validating IUT algorithm responses against the server's expected values.
    /// </summary>
    /// <typeparam name="TTestGroup">Specific test group type in which to perform validation</typeparam>
    /// <typeparam name="TTestCase">Specific test case type in which to perform validation</typeparam>
    public interface IResultValidatorAsync<TTestGroup, out TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Perform validation utilizing provided <see cref="ITestCaseValidatorAsync{TTestGroup, TTestCase}"/> against IUT provided <see cref="TTestCase"/>s
        /// </summary>
        /// <param name="testCaseValidators">The validators to use for each test case</param>
        /// <param name="testResults">The IUT supplied results to validate</param>
        /// <param name="showExpected">Shows the expected result in the validation file</param>
        /// <returns></returns>
        TestVectorValidation ValidateResults(IEnumerable<ITestCaseValidatorAsync<TTestGroup, TTestCase>> testCaseValidators, IEnumerable<TTestGroup> testResults, bool showExpected);
    }
}