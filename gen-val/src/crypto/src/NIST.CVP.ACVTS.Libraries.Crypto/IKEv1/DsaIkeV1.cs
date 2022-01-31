using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.IKEv1
{
    public class DsaIkeV1 : IkeV1Base
    {
        public DsaIkeV1(IHmac hmac) : base(hmac) { }

        protected override BitString PseudoRandomFunction(BitString ni, BitString nr, BitString gxy = null, BitString cky_i = null, BitString cky_r = null, BitString presharedKey = null)
        {
            return Hmac.Generate(ni.ConcatenateBits(nr), gxy).Mac;
        }
    }
}
