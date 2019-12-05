using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.v1_0.ECC
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        protected readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var result = await _oracle.GetKasAftTestEccAsync(
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
                    EphemeralKeyIut = new EccKeyPair(
                        new EccPoint(result.EphemeralPublicKeyIutX, result.EphemeralPublicKeyIutY), 
                        result.EphemeralPrivateKeyIut), 
                    StaticKeyIut = new EccKeyPair(
                        new EccPoint(result.StaticPublicKeyIutX, result.StaticPublicKeyIutY), 
                        result.StaticPrivateKeyIut),
                    EphemeralKeyServer = new EccKeyPair(
                        new EccPoint(result.EphemeralPublicKeyServerX, result.EphemeralPublicKeyServerY), 
                        result.EphemeralPrivateKeyServer), 
                    StaticKeyServer = new EccKeyPair(
                        new EccPoint(result.StaticPublicKeyServerX, result.StaticPublicKeyServerY), 
                        result.StaticPrivateKeyServer),
                    HashZ = result.HashZ,
                    IdIut = result.IdIut,
                    IdIutLen = result.IdIut?.BitLength ?? 0,
                    MacData = result.MacData,
                    NonceAesCcm = result.NonceAesCcm,
                    NonceNoKc = result.NonceNoKc,
                    OiLen = result.OiLen,
                    OtherInfo = result.OtherInfo,
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
        
        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}