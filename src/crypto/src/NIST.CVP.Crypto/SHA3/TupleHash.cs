using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.SHA3
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

        public HashResult HashMessage(HashFunction hashFunction, List<BitString> tuples, string customization = "")
        {
            try
            {
                var sha = _iTupleHashFactory.GetTupleHash(hashFunction);
                var digest = sha.HashMessage(tuples, hashFunction.DigestSize, hashFunction.Capacity, hashFunction.XOF, customization);

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
