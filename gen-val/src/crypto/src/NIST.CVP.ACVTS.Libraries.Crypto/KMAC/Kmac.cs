using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KMAC
{
    public class Kmac : IKmac
    {
        private const string FunctionName = "KMAC";
        private readonly ICSHAKEWrapper _iCSHAKE;
        private int _capacity;
        private bool _xof;

        public Kmac(ICSHAKEWrapper iCSHAKE, int capacity, bool xof)
        {
            // Guard against invalid capacities
            if (capacity != 512 && capacity != 256)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _iCSHAKE = iCSHAKE;
            _capacity = capacity;
            _xof = xof;
        }

        int IMac.OutputLength => _capacity;

        public MacResult Generate(BitString key, BitString message, string customization, int macLength = 0)
        {
            if (macLength == 0)
            {
                macLength = _capacity;
            }

            var newMessage = KmacHelpers.FormatMessage(message, key, _capacity, macLength, _xof);

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, customization, FunctionName));
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            return Generate(key, message, "", macLength);
        }

        #region Hex Customization
        public MacResult Generate(BitString key, BitString message, BitString customizationHex, int macLength = 0)
        {
            if (macLength == 0)
            {
                macLength = _capacity;
            }

            var newMessage = KmacHelpers.FormatMessage(message, key, _capacity, macLength, _xof);

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, customizationHex, FunctionName));
        }
        #endregion Hex Customization
    }
}
