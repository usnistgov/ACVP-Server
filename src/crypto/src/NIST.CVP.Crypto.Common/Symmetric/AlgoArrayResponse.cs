using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class AlgoArrayResponse : IAlgoArrayResponse
    {
        public BitString Key { get; set; }
        public BitString IV { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
    }
}