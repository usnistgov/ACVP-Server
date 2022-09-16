using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KMAC
{
    public class Kmac : IKmac
    {
        private const string FunctionName = "KMAC";
        private readonly IcSHAKEWrapper _icSHAKE;
        private int _capacity;
        private bool _xof;

        public Kmac(IcSHAKEWrapper icSHAKE, int capacity, bool xof)
        {
            // Guard against invalid capacities
            if (capacity != 512 && capacity != 256)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _icSHAKE = icSHAKE;
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

            return new MacResult(_icSHAKE.HashMessage(newMessage, macLength, _capacity, customization, FunctionName));
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

            return new MacResult(_icSHAKE.HashMessage(newMessage, macLength, _capacity, customizationHex, FunctionName));
        }
        #endregion Hex Customization
    }
}
