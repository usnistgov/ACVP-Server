using NIST.CVP.ACVTS.Libraries.Generation.Core;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public BlockCipherDirections[] Directions { get; set; } = { };
        public MathDomain PayloadLength { get; set; }
        public MathDomain ADLength { get; set; }
        public MathDomain TagLength { get; set; }
        public bool[] SupportsNonceMasking { get; set; } = { };
    }
}
