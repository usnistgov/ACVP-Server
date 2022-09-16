using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF
{
    public class KmacKdf : KdfBase, IKdf
    {
        private readonly IKmac _kmac;

        public KmacKdf(IMac mac) : base(mac)
        {
            _kmac = (Kmac)mac;  // Hard assumption that the type being provided by the factory is a KMAC
        }

        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0)
        {
            var derivedKey = _kmac.Generate(kI, fixedData, iv, len);
            return new KdfResult(derivedKey.Mac);
        }
    }
}
