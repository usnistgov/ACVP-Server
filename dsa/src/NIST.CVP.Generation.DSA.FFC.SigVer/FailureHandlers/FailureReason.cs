using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers
{
    public class FailureReason : IFailureReason<SigFailureReasons>
    {
        private readonly SigFailureReasons _reason;

        public FailureReason(SigFailureReasons reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelper.SigFailureReasonToString(_reason);
        }

        public SigFailureReasons GetReason()
        {
            return _reason;
        }
    }
}
