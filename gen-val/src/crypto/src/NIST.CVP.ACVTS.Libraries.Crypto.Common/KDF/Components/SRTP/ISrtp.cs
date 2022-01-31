using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP
{
    public interface ISrtp
    {
        KdfResult DeriveKey(int keyLength, BitString keyMaster, BitString saltMaster, BitString kdr, BitString index, BitString srtcpIndex);
    }
}
