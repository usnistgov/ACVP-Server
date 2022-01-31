using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.IKEv1
{
    public abstract class IkeV1Base : IIkeV1
    {
        protected readonly IHmac Hmac;

        protected IkeV1Base(IHmac hmac)
        {
            Hmac = hmac;
        }

        public IkeResult GenerateIke(BitString ni, BitString nr, BitString gxy, BitString cky_i, BitString cky_r, BitString presharedKey = null)
        {
            // Append fixed values together for convenience
            var fixedData = gxy.ConcatenateBits(cky_i).ConcatenateBits(cky_r);

            var sKeyId = PseudoRandomFunction(ni, nr, gxy, cky_i, cky_r, presharedKey);

            var sKeyIdD = Hmac.Generate(sKeyId, fixedData.ConcatenateBits(new BitString("00"))).Mac;
            var sKeyIdA = Hmac.Generate(sKeyId, sKeyIdD.ConcatenateBits(fixedData).ConcatenateBits(new BitString("01"))).Mac;
            var sKeyIdE = Hmac.Generate(sKeyId, sKeyIdA.ConcatenateBits(fixedData).ConcatenateBits(new BitString("02"))).Mac;

            return new IkeResult(sKeyId, sKeyIdA, sKeyIdD, sKeyIdE);
        }

        protected abstract BitString PseudoRandomFunction(BitString ni, BitString nr, BitString gxy = null,
            BitString cky_i = null, BitString cky_r = null, BitString presharedKey = null);
    }
}
