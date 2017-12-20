using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.IKEv1
{
    public class PkeIkeV1 : IkeV1Base
    {
        private readonly ISha _sha;

        public PkeIkeV1(IHmac hmac, ISha sha) : base(hmac)
        {
            _sha = sha;
        }

        protected override BitString PseudoRandomFunction(BitString ni, BitString nr, BitString gxy = null, BitString cky_i = null, BitString cky_r = null, BitString presharedKey = null)
        {
            var key = _sha.HashMessage(ni.ConcatenateBits(nr)).Digest;
            return Hmac.Generate(key, cky_i.ConcatenateBits(cky_r)).Mac;
        }
    }
}
