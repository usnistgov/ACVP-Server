using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TupleHash
{
    public class TupleHashWrapper : ITupleHashWrapper
    {
        private const string FunctionName = "TupleHash";
        private List<BitString> _tuple;
        private CSHAKEWrapper _cSHAKE;

        public virtual BitString HashMessage(IEnumerable<BitString> tuple, int digestLength, int capacity, bool xof, string customization = "")
        {
            Init();
            Update(tuple);
            return Final(digestLength, capacity, xof, customization);
        }

        // These functions are for portability
        private void Init()
        {
            _tuple = new List<BitString>();
            _cSHAKE = new CSHAKEWrapper();
        }

        private void Update(IEnumerable<BitString> newContent)
        {
            _tuple.AddRange(newContent);
        }

        private BitString Final(int digestLength, int capacity, bool xof, string customization)
        {
            var newMessage = TupleHashHelpers.FormatMessage(_tuple, digestLength, xof);
            return _cSHAKE.HashMessage(newMessage, digestLength, capacity, customization, FunctionName);
        }

        #region BitString Customization
        public BitString HashMessage(IEnumerable<BitString> tuple, int digestLength, int capacity, bool xof, BitString customizationHex)
        {
            Init();
            Update(tuple);
            return Final(digestLength, capacity, xof, customizationHex);
        }

        private BitString Final(int digestLength, int capacity, bool xof, BitString customizationHex)
        {
            var newMessage = TupleHashHelpers.FormatMessage(_tuple, digestLength, xof);
            return _cSHAKE.HashMessage(newMessage, digestLength, capacity, customizationHex, FunctionName);
        }
        #endregion BitString Customization
    }
}
