using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.SHA3
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

        public HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, string customization = "")
        {
            try
            {
                var sha = _iParallelHashFactory.GetParallelHash(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestSize, hashFunction.Capacity, blockSize, hashFunction.XOF, customization);

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
