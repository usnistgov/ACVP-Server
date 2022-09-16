using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class KdfKmacParameters
    {
        public int KeyDerivationKeyLength { get; set; }
        public int DerivedKeyLength { get; set; }
        public int ContextLength { get; set; }
        public int LabelLength { get; set; }
        public MacModes MacMode { get; set; }
    }
}
