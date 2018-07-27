using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.KMAC
{
    public class Kmac : IKmac
    {
        private readonly ICSHAKEWrapper _iCSHAKE;
        private int _capacity;
        private bool _xof;

        public Kmac(ICSHAKEWrapper iCSHAKE, int capacity, bool xof)
        {
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

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, customization, "KMAC"));
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

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, customizationHex, "KMAC"));
        }
        #endregion Hex Customization
    }
}
