using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KMAC
{
    public class KmacFactory : IKmacFactory
    {
        private readonly IcSHAKEWrapper _icSHAKEWrapper;

        public KmacFactory(IcSHAKEWrapper icSHAKEWrapper)
        {
            _icSHAKEWrapper = icSHAKEWrapper;
        }

        public IKmac GetKmacInstance(int capacity, bool xof)
        {
            return new Kmac(_icSHAKEWrapper, capacity, xof);
        }
    }
}
