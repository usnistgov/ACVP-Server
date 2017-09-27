using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public class PQFailureHandler : IFailureHandler
    {
        private List<PQFailureReason> _failureReasons;

        public PQFailureHandler(bool isSample = false)
        {
            _failureReasons = new List<PQFailureReason>();

            // For a sample case, we always want a 'None' and a random bad reason
            if (isSample)
            {
                var badReasons = new List<PQFailureReason>
                {
                    new PQFailureReason(PQFailureReasons.ModifyP),
                    new PQFailureReason(PQFailureReasons.ModifyQ),
                    new PQFailureReason(PQFailureReasons.ModifySeed)
                };

                _failureReasons.Add(new PQFailureReason(PQFailureReasons.None));
                _failureReasons.Add(badReasons.OrderBy(a => Guid.NewGuid()).ToList()[0]);
            }
            // Otherwise we want everything
            else
            {
                _failureReasons.Add(new PQFailureReason(PQFailureReasons.None), 2);

                _failureReasons.Add(new PQFailureReason(PQFailureReasons.ModifyP));
                _failureReasons.Add(new PQFailureReason(PQFailureReasons.ModifyQ));
                _failureReasons.Add(new PQFailureReason(PQFailureReasons.ModifySeed));
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
