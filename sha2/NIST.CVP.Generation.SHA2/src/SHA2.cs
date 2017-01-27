using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public class SHA2 : ISHA2
    {
        private readonly ISHAFactory _iSHAFactory;

        public SHA2(ISHAFactory iSHAFactory)
        {
            _iSHAFactory = iSHAFactory;
        }

        public SHA2()
        {
            _iSHAFactory = new SHAFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            try
            {
                var sha = _iSHAFactory.GetSHA(hashFunction.Mode);
                var digest = sha.HashMessage(hashFunction, message);

                return new HashResult(digest);
            }
            catch(Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult(ex.Message);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
