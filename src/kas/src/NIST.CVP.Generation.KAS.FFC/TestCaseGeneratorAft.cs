using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        protected readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            try
            {
                var result = _oracle.GetKasAftTestFfc(
                    new KasAftParametersFfc()
                    {
                        P = group.P,
                        Q = group.Q,
                        G = group.G,
                        AesCcmNonceLen = group.AesCcmNonceLen,
                        FfcParameterSet = group.ParmSet,
                        FfcScheme = group.Scheme,
                        HashFunction = group.HashAlg,
                        IdServer = SpecificationMapping.ServerId,
                        IutKeyAgreementRole = group.KasRole,
                        IutKeyConfirmationRole = group.KcRole,
                        KasMode = group.KasMode,
                        KeyConfirmationDirection = group.KcType,
                        KeyLen = group.KeyLen,
                        MacLen = group.MacLen,
                        MacType = group.MacType,
                        OiPattern = group.OiPattern,
                        IsSample = isSample
                    }
                );

                var testCase = new TestCase()
                {
                    TestPassed = true,
                    Deferred = result.Deferred,
                    Dkm = result.Dkm,
                    DkmNonceIut = result.DkmNonceIut,
                    DkmNonceServer = result.DkmNonceServer,
                    EphemeralNonceIut = result.EphemeralNonceIut,
                    EphemeralNonceServer = result.EphemeralNonceServer,
                    EphemeralPrivateKeyIut = result.EphemeralPrivateKeyIut,
                    EphemeralPrivateKeyServer = result.EphemeralPrivateKeyServer,
                    EphemeralPublicKeyIut = result.EphemeralPublicKeyIut,
                    EphemeralPublicKeyServer = result.EphemeralPublicKeyServer,
                    HashZ = result.HashZ,
                    IdIut = result.IdIut,
                    IdIutLen = result.IdIut?.BitLength ?? 0,
                    MacData = result.MacData,
                    NonceAesCcm = result.NonceAesCcm,
                    NonceNoKc = result.NonceNoKc,
                    OiLen = result.OiLen,
                    OtherInfo = result.OtherInfo,
                    StaticPrivateKeyIut = result.StaticPrivateKeyIut,
                    StaticPrivateKeyServer = result.StaticPrivateKeyServer,
                    StaticPublicKeyIut = result.StaticPublicKeyIut,
                    StaticPublicKeyServer = result.StaticPublicKeyServer,
                    Tag = result.Tag,
                    Z = result.Z
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}