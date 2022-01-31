using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
{
    public interface IFixedInfoStrategyFactory
    {
        IFixedInfoStrategy Get(FixedInfoEncoding encoding);
    }
}
