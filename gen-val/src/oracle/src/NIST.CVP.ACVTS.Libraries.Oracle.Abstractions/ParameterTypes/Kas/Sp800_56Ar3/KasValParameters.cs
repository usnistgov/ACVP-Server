using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasValParameters : KasParametersBase
    {
        public KasValTestDisposition Disposition { get; set; }
        public BitString PartyIdIut { get; set; }
        public int L { get; set; }

        public IKdfConfiguration KdfConfiguration { get; set; }
        public MacConfiguration MacConfiguration { get; set; }

        public IDsaKeyPair ServerEphemeralKey { get; set; }
        public IDsaKeyPair ServerStaticKey { get; set; }

        public IDsaKeyPair IutEphemeralKey { get; set; }
        public IDsaKeyPair IutStaticKey { get; set; }
    }
}
