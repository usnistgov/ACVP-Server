using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    public interface IFixedInfoStrategyFactory
    {
        IFixedInfoStrategy Get(KasKdfFixedInfoEncoding encoding);
    }
}