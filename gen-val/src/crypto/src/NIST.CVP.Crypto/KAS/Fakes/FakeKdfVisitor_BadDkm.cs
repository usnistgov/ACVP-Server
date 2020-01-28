using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    public class FakeKdfVisitor_BadDkm : IKdfVisitor
    {
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IRandom800_90 _random;

        public FakeKdfVisitor_BadDkm(IKdfVisitor kdfVisitor, IRandom800_90 random)
        {
            _kdfVisitor = kdfVisitor;
            _random = random;
        }

        public KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }

        public KdfResult Kdf(KdfParameterTwoStep param, BitString fixedInfo)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }

        public KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }

        public KdfResult Kdf(KdfParameterIkeV2 param, BitString fixedInfo = null)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }

        public KdfResult Kdf(KdfParameterTls10_11 param, BitString fixedInfo = null)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }

        public KdfResult Kdf(KdfParameterTls12 param, BitString fixedInfo = null)
        {
            var result = _kdfVisitor.Kdf(param, fixedInfo);
            var dkmBytesLen = result.DerivedKey.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // Modify a random byte within DKM
            result.DerivedKey[_random.GetRandomInt(0, dkmBytesLen)] += 2;

            return result;
        }
    }
}