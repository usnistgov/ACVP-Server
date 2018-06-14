using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.SHA3
{
    public class TupleHashWrapper : ITupleHashWrapper
    {
        private BitString _message;
        private CSHAKEWrapper _cSHAKE;

        public virtual BitString HashMessage(List<BitString> tuples, int digestSize, int capacity, bool xof, string customization = "")
        {
            Init();
            Update(tuples);
            return Final(digestSize, capacity, xof, customization);
        }

        // These functions are for portability
        private void Init()
        {
            _message = new BitString(0);
            _cSHAKE = new CSHAKEWrapper();
        }

        private void Update(List<BitString> tuples)
        {
            foreach(BitString tuple in tuples)
            {
                _message = BitString.ConcatenateBits(_message, CSHAKEHelpers.EncodeString(tuple));
            }
        }

        private BitString Final(int digestSize, int capacity, bool xof, string customization)
        {
            _message = BitString.ConcatenateBits(_message, CSHAKEHelpers.RightEncode(new BitString(new System.Numerics.BigInteger(digestSize))));

            return _cSHAKE.HashMessage(_message, digestSize, capacity, xof, "TupleHash", customization);
        }
    }
}
