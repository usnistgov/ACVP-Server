using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.MD5
{
    public interface IMd5
    {
        HashResult Hash(BitString message);
    }
}
