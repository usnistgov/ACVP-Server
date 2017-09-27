using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public class GFailureHandler : IFailureHandler
    {
        private List<GFailureReason> _failureReasons;

        public GFailureHandler(bool isSample = false)
        {
            _failureReasons = new List<GFailureReason>();

            // For a sample case, we always want a 'None' and a 'Modify G'
            if (isSample)
            {
                _failureReasons.Add(new GFailureReason(GFailureReasons.None));
                _failureReasons.Add(new GFailureReason(GFailureReasons.ModifyG));
            }
            // Otherwise we want everything
            else
            {
                _failureReasons.Add(new GFailureReason(GFailureReasons.None), 2);
                _failureReasons.Add(new GFailureReason(GFailureReasons.ModifyG), 3);
            }
        }
        public IFailureReason GetNextFailureReason()
        {
            var shuffledReasons = _failureReasons.OrderBy(a => Guid.NewGuid()).ToList();
            var reason = shuffledReasons[0];
            _failureReasons.Remove(reason);

            return reason;
        }
    }
}
