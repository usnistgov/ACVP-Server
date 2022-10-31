using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class IkeV2KdfParameters
    {
        public int NInitLength { get; set; }
        public int NRespLength { get; set; }
        public int GirLength { get; set; }
        public HashFunction HashAlg { get; set; }
        public int DerivedKeyingMaterialLength { get; set; }
        public int DerivedKeyingMaterialChildLength { get; set; }
    }
}
