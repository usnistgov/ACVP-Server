using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.SHA3
{
    public class CSHAKE : ICSHAKE
    {
        private readonly ICSHAKEFactory _iCSHAKEFactory;

        public CSHAKE(ICSHAKEFactory iCSHAKEFactory)
        {
            _iCSHAKEFactory = iCSHAKEFactory;
        }

        public CSHAKE()
        {
            _iCSHAKEFactory = new CSHAKEFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message, string functionName, string customization)
        {
            try
            {
                var sha = _iCSHAKEFactory.GetCSHAKE(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestSize, hashFunction.Capacity, hashFunction.XOF, functionName, customization);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            return HashMessage(hashFunction, message, "", "");
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
