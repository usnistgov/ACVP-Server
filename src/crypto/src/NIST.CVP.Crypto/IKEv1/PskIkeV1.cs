﻿using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.IKEv1
{
    public class PskIkeV1 : IkeV1Base
    {
        public PskIkeV1(IHmac hmac) : base(hmac) { }

        protected override BitString PseudoRandomFunction(BitString ni, BitString nr, BitString gxy = null, BitString cky_i = null, BitString cky_r = null, BitString presharedKey = null)
        {
            return Hmac.Generate(presharedKey, ni.ConcatenateBits(nr)).Mac;
        }
    }
}
