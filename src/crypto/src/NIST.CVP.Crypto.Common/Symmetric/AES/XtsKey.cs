using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public class XtsKey
    {
        public BitString Key { get; }

        public BitString Key1 => BitString.MSBSubstring(Key, 0, Key.BitLength / 2);
        public BitString Key2 => BitString.Substring(Key, 0, Key.BitLength / 2);

        public XtsKey(BitString key)
        {
            Key = key;
            CheckKey();
        }
        
        private void CheckKey()
        {
            if (Key.BitLength != 256 && Key.BitLength != 512)
            {
                // throw new ArgumentException("Invalid key length in XTS");
            }
        }
    }
}
