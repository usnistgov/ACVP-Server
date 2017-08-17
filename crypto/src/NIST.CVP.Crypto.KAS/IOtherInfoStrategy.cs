using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public interface IOtherInfoStrategy
    {
        BitString GetOtherInfo();
    }
}