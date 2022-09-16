using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class KdfKmacResult
    {
        public BitString DerivedKey { get; set; }
        public BitString KeyDerivationKey { get; set; }
        public BitString Context { get; set; }
        public BitString Label { get; set; }
    }
}
