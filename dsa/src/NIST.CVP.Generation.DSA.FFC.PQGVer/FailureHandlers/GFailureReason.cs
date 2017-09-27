using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public class GFailureReason : IFailureReason
    {
        public GFailureReasons Reason { get; set; }

        public GFailureReason(GFailureReasons reason)
        {
            Reason = reason;
        }

        public string GetName()
        {
            return EnumHelper.ReasonToString(Reason);
        }
    }
}
