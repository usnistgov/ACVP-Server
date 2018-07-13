using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.KAS.ECC
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
                var result = _oracle.GetKasAftTestEcc(
                    new KasAftParametersEcc()
                    {
                        AesCcmNonceLen = group.AesCcmNonceLen,
                        Curve = group.Curve,
                        EccParameterSet = group.ParmSet,
                        EccScheme = group.Scheme,
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
                    EphemeralPublicKeyIutX = result.EphemeralPublicKeyIutX,
                    EphemeralPublicKeyIutY = result.EphemeralPublicKeyIutY,
                    EphemeralPublicKeyServerX = result.EphemeralPublicKeyServerX,
                    EphemeralPublicKeyServerY = result.EphemeralPublicKeyServerY,
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
                    StaticPublicKeyIutX = result.StaticPublicKeyIutX,
                    StaticPublicKeyIutY = result.StaticPublicKeyIutY,
                    StaticPublicKeyServerX = result.StaticPublicKeyServerX,
                    StaticPublicKeyServerY = result.StaticPublicKeyServerY,
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
            throw new System.NotImplementedException();
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}