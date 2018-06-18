using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.SHA3;
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

            var macLengthBitString = new BitString(new System.Numerics.BigInteger(macLength));
            
            BitString newMessage;
            if (_capacity == 256)
            {
                newMessage = SHA3DerivedHelpers.Bytepad(SHA3DerivedHelpers.EncodeString(key), new BitString("A8"));   // "A8" is 164 (the rate)
            }
            else      // capacity == 512
            {
                newMessage = SHA3DerivedHelpers.Bytepad(SHA3DerivedHelpers.EncodeString(key), new BitString("88"));   // "88" is 136 (the rate)
            }
            
            if (_xof)
            {
                newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, SHA3DerivedHelpers.RightEncode(BitString.Zeroes(8))));
            }
            else
            {
                newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, SHA3DerivedHelpers.RightEncode(macLengthBitString)));
            }

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, "KMAC", customization));
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            return Generate(key, message, "", macLength);
        }
    }
}
