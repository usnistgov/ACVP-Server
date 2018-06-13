using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KMAC
{
    public class Kmac : IKmac
    {
        private readonly ICSHAKEWrapper _iCSHAKE;
        private int _capacity;
        private int _outputLength;

        public Kmac(ICSHAKEWrapper iCSHAKE, int capacity, int outputLength)
        {
            _iCSHAKE = iCSHAKE;
            _capacity = capacity;
            _outputLength = outputLength;
        }

        int IMac.OutputLength => _outputLength;

        public MacResult Generate(BitString key, BitString message, string customization, int macLength = 0)
        {
            var macLengthBitString = new BitString(new System.Numerics.BigInteger(macLength));

            BitString newMessage;
            if (_capacity == 256)
            {
                newMessage = CSHAKEHelpers.Bytepad(CSHAKEHelpers.EncodeString(key), new BitString("A8"));
            }
            else      // capacity == 512
            {
                newMessage = CSHAKEHelpers.Bytepad(CSHAKEHelpers.EncodeString(key), new BitString("88"));
            }
            
            newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, CSHAKEHelpers.RightEncode(macLengthBitString)));

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, _capacity, true, "KMAC", customization));
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            return Generate(key, message, "", macLength);
        }
    }
}
