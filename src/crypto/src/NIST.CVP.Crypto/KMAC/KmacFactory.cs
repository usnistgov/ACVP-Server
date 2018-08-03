using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.Crypto.KMAC
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
