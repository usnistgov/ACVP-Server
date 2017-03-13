using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        public int[] KeyLen { get; set; }
        public Range PtLen { get; set; }
        public int[] Nonce { get; set; }

        private Range _aadLen;
        public Range AadLen
        {
            get { return _aadLen; }
            set
            {
                SupportsAad2Pow16 = value.Max == 65536;

                if (value.Min > 32)
                {
                    value.Min = 32;
                }

                if (value.Max > 32)
                {
                    value.Max = 32;
                }

                _aadLen = value;
            }
        }
        public int[] TagLen { get; set; }
        public bool SupportsAad2Pow16 { get; set; }
    }
}
