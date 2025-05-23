﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE
{
    public class cSHAKEWrapper : IcSHAKEWrapper
    {
        private BitString _message;

        public virtual BitString HashMessage(BitString message, int digestLength, int capacity, string customization, string functionName)
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, customization, functionName);
        }

        public virtual BitString HashMessage(BitString message, int digestLength, int capacity)
        {
            return HashMessage(message, digestLength, capacity, "", "");
        }

        // These functions are for portability
        private void Init()
        {
            _message = new BitString(0);
        }

        private void Update(BitString newContent)
        {
            _message = BitString.ConcatenateBits(_message, newContent);
        }

        private BitString Final(int digestLength, int capacity, string customization, string functionName)
        {
            if (functionName.Equals("") && customization.Equals(""))
            {
                return KeccakInternals.Keccak(_message, digestLength, capacity, true);
            }

            var formattedMessage = cSHAKEHelpers.FormatMessage(_message, capacity, functionName, customization);

            return KeccakInternals.Keccak(formattedMessage, digestLength, capacity, true, true);
        }

        #region BitString Customization
        public virtual BitString HashMessage(BitString message, int digestLength, int capacity, BitString customizationHex, string functionName)
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, functionName, customizationHex);
        }

        private BitString Final(int digestLength, int capacity, string functionName, BitString customizationHex)
        {
            if (functionName.Equals("") && BitString.Equals(customizationHex, new BitString(0)))
            {
                return KeccakInternals.Keccak(_message, digestLength, capacity, true);
            }

            var formattedMessage = cSHAKEHelpers.FormatMessage(_message, capacity, functionName, customizationHex);

            return KeccakInternals.Keccak(formattedMessage, digestLength, capacity, true, true);
        }
        #endregion BitString Customization
    }
}
