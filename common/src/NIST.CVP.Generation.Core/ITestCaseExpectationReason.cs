using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseExpectationReason<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        TEnum GetReason();
        string GetName();
    }
}
