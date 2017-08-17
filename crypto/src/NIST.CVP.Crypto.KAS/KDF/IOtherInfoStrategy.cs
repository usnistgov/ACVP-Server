using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public interface IOtherInfoStrategy
    {
        BitString GetOtherInfo();
    }
}