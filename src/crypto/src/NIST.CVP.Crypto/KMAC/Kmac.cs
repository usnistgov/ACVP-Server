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

        public Kmac(ICSHAKEWrapper iCSHAKE)
        {
            _iCSHAKE = iCSHAKE;
        }

        int IMac.OutputLength => throw new System.NotImplementedException();

        public MacResult Generate(BitString key, BitString message, string customization, int macLength = 0)
        {
            var macLengthBitString = new BitString(new System.Numerics.BigInteger(macLength));
            var newMessage = CSHAKEHelpers.Bytepad(CSHAKEHelpers.EncodeString(key), new BitString("A8"));
            newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, CSHAKEHelpers.RightEncode(macLengthBitString)));

            return new MacResult(_iCSHAKE.HashMessage(newMessage, macLength, 256, true, "KMAC", customization));
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            return Generate(key, message, "", macLength);
        }
    }
}
