using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Generation.SHA;
using NLog;

namespace NIST.CVP.Generation.SHA1
{
    public class SHA1 : ISHA1
    {
        private readonly ISHAFactory _iSHAFactory;

        public SHA1(ISHAFactory iSHAFactory)
        {
            _iSHAFactory = iSHAFactory;
        }

        public HashResult HashMessage(BitString message)
        {
            try
            {
                var mode = ModeValues.SHA1;                 // Only one way to do SHA1
                var sha = _iSHAFactory.GetSHA(mode);
                var hashFunction = new HashFunction { DigestSize = 160, Mode = mode };
                var digest = sha.HashMessage(hashFunction, message);

                ThisLogger.Debug($"mode: {hashFunction.Mode}");
                ThisLogger.Debug($"digest size: {hashFunction.DigestSize}");
                ThisLogger.Debug($"message: {message.ToHex()}");
                ThisLogger.Debug($"digest: {digest.ToHex()}");

                return new HashResult(digest);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new HashResult($"Some Error Message: {ex.StackTrace}");
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
