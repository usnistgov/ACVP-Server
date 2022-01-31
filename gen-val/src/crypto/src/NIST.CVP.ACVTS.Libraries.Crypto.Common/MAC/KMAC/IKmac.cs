using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC
{
    public interface IKmac : IMac
    {
        MacResult Generate(BitString key, BitString message, string customization, int macLength = 0);
        MacResult Generate(BitString key, BitString message, BitString customizationHex, int macLength = 0);
    }
}
