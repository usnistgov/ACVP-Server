using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
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
