using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KTS
{
    public class DecryptionFailedException : Exception
    {
        public DecryptionFailedException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
