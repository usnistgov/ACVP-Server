using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class EddsaSignatureParameters
    {
        public bool PreHash { get; set; }
        public EdKeyPair Key { get; set; }
        public Curve Curve { get; set; }
        public EddsaSignatureDisposition Disposition { get; set; }
        public BitString Message { get; set; }
        public int Bit { get; set; }
    }
}
