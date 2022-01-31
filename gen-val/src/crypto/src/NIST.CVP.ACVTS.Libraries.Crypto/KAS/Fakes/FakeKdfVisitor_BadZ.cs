using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes
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

        public KdfResult Kdf(KdfParameterOneStepNoCounter param, BitString fixedInfo)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterOneStepNoCounter()
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

        public KdfResult Kdf(KdfParameterTwoStep param, BitString fixedInfo)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterTwoStep()
            {
                L = param.L,
                Salt = param.Salt,
                Z = param.Z.GetDeepCopy(),
                FixedInfoPattern = param.FixedInfoPattern,
                FixedInputEncoding = param.FixedInputEncoding,
                MacMode = param.MacMode,
                KdfMode = param.KdfMode,
                Iv = param.Iv,
                CounterLocation = param.CounterLocation,
                CounterLen = param.CounterLen
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }

        public KdfResult Kdf(KdfParameterHkdf param, BitString fixedInfo)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterIkeV1()
            {
                L = param.L,
                Z = param.Z.GetDeepCopy(),
                HashFunction = param.HmacAlg
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }

        public KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterIkeV1()
            {
                L = param.L,
                Z = param.Z.GetDeepCopy(),
                HashFunction = param.HashFunction
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }

        public KdfResult Kdf(KdfParameterIkeV2 param, BitString fixedInfo = null)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterIkeV1()
            {
                L = param.L,
                Z = param.Z.GetDeepCopy(),
                HashFunction = param.HashFunction
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }

        public KdfResult Kdf(KdfParameterTls10_11 param, BitString fixedInfo = null)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterTls10_11()
            {
                L = param.L,
                Z = param.Z.GetDeepCopy()
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }

        public KdfResult Kdf(KdfParameterTls12 param, BitString fixedInfo = null)
        {
            var zBytesLen = param.Z.BitLength.CeilingDivide(BitString.BITSINBYTE);

            var modifiedParam = new KdfParameterTls12()
            {
                L = param.L,
                Z = param.Z.GetDeepCopy(),
                HashFunction = param.HashFunction
            };

            // Modify a random byte within Z
            modifiedParam.Z[_random.GetRandomInt(0, zBytesLen)] += 2;

            return _kdfVisitor.Kdf(modifiedParam, fixedInfo);
        }
    }
}
