using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SNMP
{
    public class Snmp : ISnmp
    {
        private readonly ISha _sha;
        private const int BITS_IN_MEGABYTE = 1024 * 1024 * 8;

        public Snmp(ISha sha)
        {
            _sha = sha;
        }

        public SnmpResult KeyLocalizationFunction(BitString snmpEngineId, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new SnmpResult("Need a password");
            }

            if (snmpEngineId.BitLength < 40 || snmpEngineId.BitLength > 256)
            {
                return new SnmpResult("Snmp Engine ID must be between 8-32 bytes");
            }

            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var expandedPassword = new BitString(passwordBytes, BITS_IN_MEGABYTE);

            var derivedPassword = _sha.HashMessage(expandedPassword).Digest;
            var hashInput = derivedPassword.ConcatenateBits(snmpEngineId).ConcatenateBits(derivedPassword);

            var sharedKey = _sha.HashMessage(hashInput).Digest;

            return new SnmpResult(sharedKey);
        }
    }
}
