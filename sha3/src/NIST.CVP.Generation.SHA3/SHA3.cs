using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class SHA3 : ISHA3
    {
        private readonly ISHA3Factory _iSHA3Factory;

        public SHA3(ISHA3Factory iSHA3Factory)
        {
            _iSHA3Factory = iSHA3Factory;
        }

        public SHA3()
        {
            _iSHA3Factory = new SHA3Factory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            try
            {
                var sha = _iSHA3Factory.GetSHA(hashFunction);
                var digest = sha.HashMessage(message, hashFunction.DigestSize, hashFunction.Capacity, hashFunction.XOF);

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
