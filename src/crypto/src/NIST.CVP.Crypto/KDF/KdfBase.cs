using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KDF
{
    public abstract class KdfBase
    {
        protected readonly IMac Mac;

        protected KdfBase(IMac mac)
        {
            Mac = mac;
        }

        protected BitString PseudoRandomFunction(BitString key, BitString data)
        {
            return Mac.Generate(key, data).Mac;
        }
    }
}
