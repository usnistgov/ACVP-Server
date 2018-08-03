using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class IkeV2KdfResult
    {
        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString Gir { get; set; }
        public BitString GirNew { get; set; }
        public BitString SpiInit { get; set; }
        public BitString SpiResp { get; set; }
        public BitString SKeySeed { get; set; }
        public BitString DerivedKeyingMaterial { get; set; }
        public BitString DerivedKeyingMaterialChild { get; set; }
        public BitString DerivedKeyingMaterialDh { get; set; }
        public BitString SKeySeedReKey { get; set; }
    }
}
