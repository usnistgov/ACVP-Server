using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash
{
    public class ParallelHash : IParallelHash
    {
        private readonly IParallelHashFactory _iParallelHashFactory;

        public ParallelHash(IParallelHashFactory iParallelHashFactory)
        {
            _iParallelHashFactory = iParallelHashFactory;
        }

        public ParallelHash()
        {
            _iParallelHashFactory = new ParallelHashFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, string customization)
        {
            try
            {
                var parallelHash = _iParallelHashFactory.GetParallelHash(hashFunction);
                var digest = parallelHash.HashMessage(message, hashFunction.DigestLength, hashFunction.Capacity, blockSize, hashFunction.XOF, customization);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        #region BitString Customization
        public HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, BitString customizationHex)
        {
            try
            {
                var parallelHash = _iParallelHashFactory.GetParallelHash(hashFunction);
                var digest = parallelHash.HashMessage(message, hashFunction.DigestLength, hashFunction.Capacity, blockSize, hashFunction.XOF, customizationHex);

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
