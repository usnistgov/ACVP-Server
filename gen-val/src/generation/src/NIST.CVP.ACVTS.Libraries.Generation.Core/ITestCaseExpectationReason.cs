using System;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    /// <summary>
    /// Represents a test case expectation
    /// </summary>
    /// <typeparam name="TEnum">Enum type containing test case expectations</typeparam>
    public interface ITestCaseExpectationReason<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Strongly typed reason
        /// </summary>
        /// <returns></returns>
        TEnum GetReason();
        /// <summary>
        /// String representation of reason
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
