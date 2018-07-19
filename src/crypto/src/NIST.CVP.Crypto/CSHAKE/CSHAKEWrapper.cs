using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CSHAKE
{
    public class CSHAKEWrapper : ICSHAKEWrapper
    {
        private BitString _message;

        public virtual BitString HashMessage(BitString message, int digestLength, int capacity, string functionName, string customization)
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, functionName, customization);
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

        private BitString Final(int digestLength, int capacity, string functionName, string customization)
        {
            if (functionName.Equals("") && customization.Equals(""))
            {
                return KeccakInternals.Keccak(_message, digestLength, capacity, true);
            }

            var formattedMessage = CSHAKEHelpers.FormatMessage(_message, capacity, functionName, customization);

            return KeccakInternals.Keccak(formattedMessage, digestLength, capacity, true, true);
        }

        #region BitString Customization
        public virtual BitString HashMessage(BitString message, int digestLength, int capacity, string functionName, BitString customization)
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, functionName, customization);
        }

        private BitString Final(int digestLength, int capacity, string functionName, BitString customization)
        {
            if (functionName.Equals("") && customization.Equals(""))
            {
                return KeccakInternals.Keccak(_message, digestLength, capacity, true);
            }

            var formattedMessage = CSHAKEHelpers.FormatMessage(_message, capacity, functionName, customization);

            return KeccakInternals.Keccak(formattedMessage, digestLength, capacity, true, true);
        }
        #endregion BitString Customization
    }
}
