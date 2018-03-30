using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.SRTP
{
    public interface ISrtp
    {
        KdfResult DeriveKey(int keyLength, BitString keyMaster, BitString saltMaster, BitString kdr, BitString index, BitString srtcpIndex);
    }
}
