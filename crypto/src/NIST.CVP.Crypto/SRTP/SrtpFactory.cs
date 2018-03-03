using NIST.CVP.Crypto.Common.KDF.Components.SRTP;

namespace NIST.CVP.Crypto.SRTP
{
    public class SrtpFactory : ISrtpFactory
    {
        public ISrtp GetInstance()
        {
            return new Srtp();
        }
    }
}