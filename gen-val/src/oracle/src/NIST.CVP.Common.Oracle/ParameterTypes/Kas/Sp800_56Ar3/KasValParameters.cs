using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasValParameters : KasParametersBase
    {
        public KasValTestDisposition Disposition { get; set; }
        public BitString IutPartyId { get; set; }
        public int L { get; set; }
        
        public MacConfiguration MacConfiguration { get; set; }
    }
}