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

        public HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, string customization)
        {
            try
            {
                var tupleHash = _iTupleHashFactory.GetTupleHash(hashFunction);
                var digest = tupleHash.HashMessage(tuple, hashFunction.DigestLength, hashFunction.Capacity, hashFunction.XOF, customization);

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        #region BitString Customization
        public HashResult HashMessage(HashFunction hashFunction, IEnumerable<BitString> tuple, BitString customizationHex)
        {
            try
            {
                var tupleHash = _iTupleHashFactory.GetTupleHash(hashFunction);
                var digest = tupleHash.HashMessage(tuple, hashFunction.DigestLength, hashFunction.Capacity, hashFunction.XOF, customizationHex);

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
