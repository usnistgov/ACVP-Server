using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3
{
    public class KasAlgoAttributes : IKasAlgoAttributes
    {
        public KasScheme KasScheme { get; }
        public KasAlgoAttributes(KasScheme kasScheme)
        {
            KasScheme = kasScheme;
        }
    }
}
