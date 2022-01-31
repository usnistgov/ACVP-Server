using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SRTP
{
    public class SrtpFactory : ISrtpFactory
    {
        public ISrtp GetInstance()
        {
            return new Srtp();
        }
    }
}
