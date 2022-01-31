using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public int[] KeyLen { get; set; }
        public BlockCipherDirections[] Direction { get; set; }
        public MathDomain PayloadLen { get; set; }
        public MathDomain DataUnitLen { get; set; }
        public XtsTweakModes[] TweakMode { get; set; }
        public bool DataUnitLenMatchesPayload { get; set; }
    }
}
