using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.MAC.KMAC
{
    public interface IKmac : IMac
    {
        MacResult Generate(BitString key, BitString message, string customization, int macLength = 0);
    }
}
