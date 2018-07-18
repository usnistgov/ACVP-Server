using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.TupleHash
{
    public class TupleHash : ITupleHash
    {
        private readonly ITupleHashFactory _iTupleHashFactory;

        public TupleHash(ITupleHashFactory iTupleHashFactory)
        {
            _iTupleHashFactory = iTupleHashFactory;
        }

        public TupleHash()
        {
            _iTupleHashFactory = new TupleHashFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple)
        {
            try
            {
                var sha = _iTupleHashFactory.GetTupleHash(hashFunction);
                var digest = sha.HashMessage(tuple, hashFunction.DigestLength, hashFunction.Capacity, hashFunction.XOF, hashFunction.Customization);

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
