using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.KDF;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasAftParameters : KasParametersBase
    {
        public IKdfConfiguration KdfConfiguration { get; set; }
    }
}