using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public interface ISHA1_MCT
    {
        MCTResult MCTHash(BitString seed);
    }
}
