using System;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class AES_CBC : IAES_CBC
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CBC(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText)
        {
            throw new NotImplementedException();
        }

        public EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString data)
        {
            throw new NotImplementedException();
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
