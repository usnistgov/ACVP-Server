using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasAftParameters : KasParametersBase
    {
        public IKdfConfiguration KdfConfiguration { get; set; }
        public IDsaKeyPair ServerEphemeralKey { get; set; }
        public IDsaKeyPair ServerStaticKey { get; set; }
    }
}
