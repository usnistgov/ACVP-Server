using NIST.CVP.ACVTS.Libraries.Generation.Core;
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

        public BlockCipherDirections[] Direction { get; set; } = [];
        public MathDomain PayloadLen { get; set; }
        public MathDomain AadLen { get; set; }
        public MathDomain TagLen { get; set; }
        public bool[] SupportsNonceMasking { get; set; } = [];
    }
}
