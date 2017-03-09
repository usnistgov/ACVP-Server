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
        public int[] PtLen { get; set; }
        public int[] Nonce { get; set; }

        private int[] _aadLen;
        public int[] AadLen
        {
            get { return _aadLen; }
            set
            {
                // 65536 is a special case and uses minimal testing
                SupportsAad2Pow16 = value.Contains(65536);
                
                // Perform testing on values only up to 32, aside from special case 65536, handled in separate property.
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 32)
                    {
                        value[i] = 32;
                    }
                }

                _aadLen = value;
            }
        }
        public int[] TagLen { get; set; }
        public bool SupportsAad2Pow16 { get; private set; }
    }
}
