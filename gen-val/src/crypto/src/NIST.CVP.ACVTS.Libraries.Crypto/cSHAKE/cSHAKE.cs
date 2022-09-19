using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE
{
    public class cSHAKE : IcSHAKE
    {
        private readonly IcSHAKEFactory _icSHAKEFactory;

        public cSHAKE(IcSHAKEFactory icSHAKEFactory)
        {
            _icSHAKEFactory = icSHAKEFactory;
        }

        public cSHAKE()
        {
            _icSHAKEFactory = new cSHAKEFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message, string customization, string functionName = "")
        {
            try
            {
                var sha = _icSHAKEFactory.GetcSHAKE(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestLength, hashFunction.Capacity, customization, functionName);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        #region BitString Customization
        public HashResult HashMessage(HashFunction hashFunction, BitString message, BitString customization, string functionName = "")
        {
            try
            {
                var sha = _icSHAKEFactory.GetcSHAKE(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestLength, hashFunction.Capacity, customization, functionName);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }
        #endregion BitString Customization

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
