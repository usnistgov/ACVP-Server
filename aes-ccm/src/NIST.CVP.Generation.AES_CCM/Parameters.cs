using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        public int[] KeyLen { get; set; }
        public int[] PtLen { get; set; }
        public int[] Nonce { get; set; }
        public int[] AadLen { get; set; }
        public bool SupportsAad2Pow16 { get; set; }
        public int[] TagLen { get; set; }
    }
}
