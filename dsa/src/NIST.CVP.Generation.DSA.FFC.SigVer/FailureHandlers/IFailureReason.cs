using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers
{
    public interface IFailureReason<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        TEnum GetReason();
        string GetName();
    }
}
