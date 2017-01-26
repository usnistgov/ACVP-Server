using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public interface ISHA1
    {
        HashResult HashMessage(BitString message);
    }
}
