using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CCM
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        public int[] KeyLen { get; set; }
        public MathDomain PtLen { get; set; }
        public MathDomain Nonce { get; set; }
        public MathDomain AadLen { get; set; }
        public MathDomain TagLen { get; set; }
        public bool SupportsAad2Pow16 { get; set; }
    }
}
