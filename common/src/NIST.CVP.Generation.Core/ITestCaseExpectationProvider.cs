using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseExpectationProvider<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Pulls the next <see cref="ITestCaseExpectationReason{TEnum}"/> from the shuffled list.
        /// </summary>
        /// <returns></returns>
        ITestCaseExpectationReason<TEnum> GetRandomReason();
    }
}
