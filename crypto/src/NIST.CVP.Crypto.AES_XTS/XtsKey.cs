using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public class XtsKey
    {
        private readonly BitString _key;

        public BitString Key1 => BitString.MSBSubstring(_key, 0, _key.BitLength / 2);
        public BitString Key2 => BitString.Substring(_key, 0, _key.BitLength / 2);

        public XtsKey(BitString key)
        {
            _key = key;
            CheckKey();
        }

        public XtsKey(BitString key1, BitString key2)
        {
            _key = BitString.ConcatenateBits(key1, key2);
            CheckKey();
        }

        private void CheckKey()
        {
            if (_key.BitLength != 256 && _key.BitLength != 512)
            {
                throw new ArgumentException("Invalid key length in XTS");
            }
        }
    }
}
