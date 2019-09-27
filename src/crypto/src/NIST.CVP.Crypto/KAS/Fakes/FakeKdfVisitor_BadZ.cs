using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    public class FakeKdfVisitor_BadZ : IKdfVisitor
    {
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IRandom800_90 _random;

        public FakeKdfVisitor_BadZ(IKdfVisitor kdfVisitor, IRandom800_90 random)
        {
            _kdfVisitor = kdfVisitor;
            _random = random;
        }
        
        public KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);
            
            var modifiedParam = new KdfParameterOneStep()
            {
                L = param.L,
                Salt = param.Salt,
                Z = param.Z.GetDeepCopy(),
                AuxFunction = param.AuxFunction,
                FixedInfoPattern = param.FixedInfoPattern,
                FixedInputEncoding = param.FixedInputEncoding
            };
            
            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }
    }
}