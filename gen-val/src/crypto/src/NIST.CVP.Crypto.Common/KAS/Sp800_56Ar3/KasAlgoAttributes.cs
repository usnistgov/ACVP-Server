using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3
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