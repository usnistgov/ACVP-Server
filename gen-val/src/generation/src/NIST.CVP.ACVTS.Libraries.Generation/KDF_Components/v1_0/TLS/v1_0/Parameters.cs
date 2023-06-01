using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public TlsModes[] TlsVersion { get; set; }
        public string[] HashAlg { get; set; }
        public MathDomain KeyBlockLength { get; set; } = new MathDomain().AddSegment(new ValueDomainSegment(1024));
        
        // Prevent writing KeyBlockLength into JSON for certain algorithms
        public bool ShouldSerializeKeyBlockLength()
        {
            return string.Join(" ", Algorithm, Mode, Revision).Equals("TLS-v1.2 KDF RFC7627", StringComparison.OrdinalIgnoreCase);
        }
    }
}
