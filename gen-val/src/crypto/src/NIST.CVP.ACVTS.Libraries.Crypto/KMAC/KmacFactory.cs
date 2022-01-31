using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KMAC
{
    public class KmacFactory : IKmacFactory
    {
        private readonly ICSHAKEWrapper _iCSHAKEWrapper;

        public KmacFactory(ICSHAKEWrapper iCSHAKEWrapper)
        {
            _iCSHAKEWrapper = iCSHAKEWrapper;
        }

        public IKmac GetKmacInstance(int capacity, bool xof)
        {
            return new Kmac(_iCSHAKEWrapper, capacity, xof);
        }
    }
}
