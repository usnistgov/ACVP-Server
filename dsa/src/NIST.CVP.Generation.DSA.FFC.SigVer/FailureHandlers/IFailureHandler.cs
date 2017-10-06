using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers
{
    public interface IFailureHandler<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Shuffle the list of FailureReasons and return the next one
        /// </summary>
        /// <returns></returns>
        IFailureReason<TEnum> GetNextFailureReason();
    }
}
