using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.SNMP
{
    public class SnmpResult
    {
        public BitString SharedKey { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public SnmpResult(BitString sharedKey)
        {
            SharedKey = sharedKey;
        }

        public SnmpResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
