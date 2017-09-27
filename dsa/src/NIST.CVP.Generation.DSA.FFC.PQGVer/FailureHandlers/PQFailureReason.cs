using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public class PQFailureReason : IFailureReason
    {
        public PQFailureReasons Reason { get; set; }

        public PQFailureReason(PQFailureReasons reason)
        {
            Reason = reason;
        }

        public string GetName()
        {
            return EnumHelper.ReasonToString(Reason);
        }
    }
}
