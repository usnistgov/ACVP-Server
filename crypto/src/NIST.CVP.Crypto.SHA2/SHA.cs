using System;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHA : ISHA
    {
        private readonly ISHAFactory _iSHAFactory;

        public SHA(ISHAFactory iSHAFactory)
        {
            _iSHAFactory = iSHAFactory;
        }

        public SHA()
        {
            _iSHAFactory = new SHAFactory();
        }

        public HashResult HashMessage(HashFunction hashFunction, BitString message)
        {
            try
            {
                var sha = _iSHAFactory.GetSHA(hashFunction);
                var digest = sha.HashMessage(message);

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
