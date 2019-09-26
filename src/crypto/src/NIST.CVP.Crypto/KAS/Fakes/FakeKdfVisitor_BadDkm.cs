using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
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
    }
}