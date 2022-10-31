using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.IKEv2
{
    public class IkeV2 : IIkeV2
    {
        private readonly IHmac _hmac;

        public IkeV2(IHmac hmac)
        {
            _hmac = hmac;
        }

        public BitString GenerateDkmIke(BitString ni, BitString nr, BitString gir, BitString spii, BitString spir, int dkmLength)
        {
            var sKeySeed = GenerateSKeySeed(ni, nr, gir);
            return ExpansionKdf(sKeySeed, ni, nr, spii, spir, dkmLength);
        }

        public IkeResult GenerateIke(BitString ni, BitString nr, BitString gir, BitString girNew, BitString spii, BitString spir, int dkmLength, int dkmChildLength)
        {
            var sKeySeed = GenerateSKeySeed(ni, nr, gir);
            var dkm = ExpansionKdf(sKeySeed, ni, nr, spii, spir, dkmLength);

            var sKeyD = dkm.GetMostSignificantBits(_hmac.OutputLength);
            var dkmChildSA = ChildSAKdf(sKeyD, ni, nr, dkmChildLength);
            var dkmChildSADh = ChildSAKdfDh(sKeyD, girNew, ni, nr, dkmChildLength);

            var sKeySeedReKey = GenerateSKeySeedReKey(sKeyD, girNew, ni, nr);

            return new IkeResult(sKeySeed, dkm, dkmChildSA, dkmChildSADh, sKeySeedReKey);
        }

        private BitString GenerateSKeySeed(BitString ni, BitString nr, BitString gir)
        {
            return _hmac.Generate(ni.ConcatenateBits(nr), gir).Mac;
        }

        private BitString GenerateSKeySeedReKey(BitString sKD, BitString girNew, BitString ni, BitString nr)
        {
            return _hmac.Generate(sKD, girNew.ConcatenateBits(ni).ConcatenateBits(nr)).Mac;
        }

        private BitString ExpansionKdf(BitString sKeySeed, BitString ni, BitString nr, BitString spii, BitString spir, int dkmLength)
        {
            var fixedData = ni.ConcatenateBits(nr).ConcatenateBits(spii).ConcatenateBits(spir);
            return GenerateKeyingMaterial(sKeySeed, fixedData, dkmLength);
        }

        private BitString ChildSAKdf(BitString sKD, BitString ni, BitString nr, int dkmChildLength)
        {
            var fixedData = ni.ConcatenateBits(nr);
            return GenerateKeyingMaterial(sKD, fixedData, dkmChildLength);
        }

        private BitString ChildSAKdfDh(BitString sKD, BitString girNew, BitString ni, BitString nr, int dkmChildLength)
        {
            var fixedData = girNew.ConcatenateBits(ni).ConcatenateBits(nr);
            return GenerateKeyingMaterial(sKD, fixedData, dkmChildLength);
        }

        private BitString GenerateKeyingMaterial(BitString key, BitString fixedData, int dkmLength)
        {
            var iterations = dkmLength.CeilingDivide(_hmac.OutputLength);

            var keyingMaterial = new BitString(0);
            var temp = new BitString(0);
            for (var i = 0; i < iterations; i++)
            {
                var valueI = temp.ConcatenateBits(fixedData).ConcatenateBits(BitString.To32BitString(i + 1).GetLeastSignificantBits(8));
                temp = _hmac.Generate(key, valueI).Mac;
                keyingMaterial = keyingMaterial.ConcatenateBits(temp);
            }

            return keyingMaterial.GetMostSignificantBits(dkmLength);
        }
    }
}
