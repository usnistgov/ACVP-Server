using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CSHAKE
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

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            try
            {
                var sha = _iCSHAKEFactory.GetCSHAKE(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestLength, hashFunction.Capacity, hashFunction.FunctionName, hashFunction.Customization);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
