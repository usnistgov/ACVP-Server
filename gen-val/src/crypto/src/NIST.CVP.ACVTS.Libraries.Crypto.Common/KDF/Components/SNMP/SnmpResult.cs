using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP
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
