using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class IkeV1KdfResult
    {
        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString CkyInit { get; set; }
        public BitString CkyResp { get; set; }
        public BitString Gxy { get; set; }
        public BitString PreSharedKey { get; set; }
        public BitString sKeyId { get; set; }
        public BitString sKeyIdD { get; set; }
        public BitString sKeyIdA { get; set; }
        public BitString sKeyIdE { get; set; }
    }
}
