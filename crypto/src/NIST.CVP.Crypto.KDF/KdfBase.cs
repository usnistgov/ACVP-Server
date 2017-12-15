using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.MAC;
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
