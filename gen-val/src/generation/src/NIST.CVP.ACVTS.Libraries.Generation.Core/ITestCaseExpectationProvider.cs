using System;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    /// <summary>
    /// Provides a means of retrieving a test case expectation.
    /// </summary>
    /// <typeparam name="TEnum">The enum type where test case expectations are contained.</typeparam>
    public interface ITestCaseExpectationProvider<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// The number of expectations that can be pulled.
        /// </summary>
        int ExpectationCount { get; }

        /// <summary>
        /// Pulls the next <see cref="ITestCaseExpectationReason{TEnum}"/> from the shuffled list.
        /// </summary>
        /// <returns></returns>
        ITestCaseExpectationReason<TEnum> GetRandomReason();
    }
}
