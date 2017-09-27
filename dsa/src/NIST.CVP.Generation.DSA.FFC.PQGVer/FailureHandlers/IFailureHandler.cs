using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public interface IFailureHandler
    {
        /// <summary>
        /// Shuffle the list of FailureReasons and return the next one
        /// </summary>
        /// <returns></returns>
        IFailureReason GetNextFailureReason();
    }
}
