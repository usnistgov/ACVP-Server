using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.MD5
{
    public interface IMd5
    {
        HashResult Hash(BitString message);
    }
}
