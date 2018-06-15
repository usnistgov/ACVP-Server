using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.TupleHash
{
    public class TupleHashWrapper : ITupleHashWrapper
    {
        private List<BitString> _tuples;
        private CSHAKEWrapper _cSHAKE;

        public virtual BitString HashMessage(IEnumerable<BitString> tuples, int digestSize, int capacity, bool xof, string customization = "")
        {
            Init();
            Update(tuples);
            return Final(digestSize, capacity, xof, customization);
        }

        // These functions are for portability
        private void Init()
        {
            _tuples = new List<BitString>();
            _cSHAKE = new CSHAKEWrapper();
        }

        private void Update(IEnumerable<BitString> newContent)
        {
            _tuples.AddRange(newContent);
        }

        private BitString Final(int digestSize, int capacity, bool xof, string customization)
        {
            var newMessage = TupleHashHelpers.FormatMessage(_tuples, digestSize, customization, xof);
            return _cSHAKE.HashMessage(newMessage, digestSize, capacity, "TupleHash", customization);
        }
    }
}
