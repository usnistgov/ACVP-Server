using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF
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
