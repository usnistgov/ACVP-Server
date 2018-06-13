using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;


namespace NIST.CVP.Crypto.SHA3
{
    public class CSHAKEWrapper : ICSHAKEWrapper
    {
        private BitString _message;

        public virtual BitString HashMessage(BitString message, int digestSize, int capacity, Output outputType, string functionName, string customization)
        {
            Init();
            Update(message);
            return Final(digestSize, capacity, outputType, functionName, customization);
        }

        public virtual BitString HashMessage(BitString message, int digestSize, int capacity, Output outputType)
        {
            return HashMessage(message, digestSize, capacity, outputType, "", "");
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

        private BitString Final(int digestSize, int capacity, Output outputType, string functionName, string customization)
        {
            if (functionName.Equals("") && customization.Equals(""))
            {
                return KeccakInternals.Keccak(_message, digestSize, capacity, outputType);
            }

            var formattedMessage = CSHAKEHelpers.FormatMessage(_message, capacity, functionName, customization);

            return KeccakInternals.Keccak(formattedMessage, digestSize, capacity, outputType, true);
        }

    }
}
