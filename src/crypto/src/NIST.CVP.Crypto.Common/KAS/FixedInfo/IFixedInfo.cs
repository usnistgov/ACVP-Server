using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    public interface IFixedInfo
    {
        BitString Get(FixedInfoParameter param);
    }
}