using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC
{
    /// <summary>
    /// Interface for HMAC operations
    /// </summary>
    public interface IHmac : IMac
    {
        void Init(byte[] key);
        void FastInit();
        void Update(byte[] message, int bitLength);
        void Final(ref byte[] output, int macLen = 0);
    }
}
