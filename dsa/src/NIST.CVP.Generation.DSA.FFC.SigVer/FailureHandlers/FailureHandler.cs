using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers
{
    public class FailureHandler : IFailureHandler<SigFailureReasons>
    {
        private List<FailureReason> _failureReasons;

        public FailureHandler(bool isSample = false)
        {
            _failureReasons = new List<FailureReason>();

            if (isSample)
            {
                _failureReasons.Add(new FailureReason(SigFailureReasons.None));
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyMessage));
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyKey));
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyR));
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyS));
            }
            else
            {
                _failureReasons.Add(new FailureReason(SigFailureReasons.None), 7);
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyMessage), 2);
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyKey), 2);
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyR), 2);
                _failureReasons.Add(new FailureReason(SigFailureReasons.ModifyS), 2);
            }
        }

        public IFailureReason<SigFailureReasons> GetNextFailureReason()
        {
            var shuffledReasons = _failureReasons.OrderBy(a => Guid.NewGuid()).ToList();
            var reason = shuffledReasons[0];
            _failureReasons.Remove(reason);

            return reason;
        }
    }
}
