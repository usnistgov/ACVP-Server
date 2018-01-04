using NIST.CVP.Crypto.SHAWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SNMP
{
    public class Snmp : ISnmp
    {
        private readonly ISha _hash;
        private const int BITS_IN_MEGABYTE = 1024 * 1024 * 8;

        public Snmp()
        {
            _hash = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
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

            var derivedPassword = _hash.HashMessage(expandedPassword).Digest;
            var hashInput = derivedPassword.ConcatenateBits(snmpEngineId).ConcatenateBits(derivedPassword);

            var sharedKey = _hash.HashMessage(hashInput).Digest;

            return new SnmpResult(sharedKey);
        }
    }
}
