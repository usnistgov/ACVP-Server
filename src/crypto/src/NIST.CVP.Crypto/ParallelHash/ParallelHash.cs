using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.ParallelHash
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

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            try
            {
                var sha = _iParallelHashFactory.GetParallelHash(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestSize, hashFunction.Capacity, hashFunction.BlockSize, hashFunction.XOF, hashFunction.Customization);

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
