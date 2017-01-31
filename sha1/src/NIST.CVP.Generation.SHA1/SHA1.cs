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

        public SHA1()
        {
            _iSHAFactory = new SHAFactory();
        }

        public HashResult HashMessage(BitString message)
        {
            try
            {
                var hashFunction = new HashFunction { DigestSize = DigestSizes.d160, Mode = ModeValues.SHA1 };
                var sha = _iSHAFactory.GetSHA(hashFunction);
                var digest = sha.HashMessage(message);

                if(digest.BitLength != 160)
                {
                    throw new Exception("Error hashing. Digest is not proper length.");
                }

                // ThisLogger.Debug($"mode: {hashFunction.Mode}");
                // ThisLogger.Debug($"digest size: {hashFunction.DigestSize}");
                // ThisLogger.Debug($"message: {message.ToHex()}");
                // ThisLogger.Debug($"digest: {digest.ToHex()}");

                return new HashResult(digest);
            }
            catch (Exception ex)
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
