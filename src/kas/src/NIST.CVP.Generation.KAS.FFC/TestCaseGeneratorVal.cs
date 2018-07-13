using System;
using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Helpers;
using NLog;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorVal : ITestCaseGenerator<TestGroup, TestCase>
    {
        protected readonly IOracle _oracle;
        private readonly List<KasValTestDisposition> _dispositionList;

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGeneratorVal(IOracle oracle, List<KasValTestDisposition> dispositionList)
        {
            _oracle = oracle;
            _dispositionList = dispositionList;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var testCaseDisposition = TestCaseDispositionHelper.GetTestCaseIntention(_dispositionList);

            try
            {
                var result = _oracle.GetKasValTestFfc(
                    new KasValParametersFfc()
                    {
                        P = group.P,
                        Q = group.Q,
                        G = group.G,
                        AesCcmNonceLen = group.AesCcmNonceLen,
                        FfcParameterSet = group.ParmSet,
                        FfcScheme = group.Scheme,
                        HashFunction = group.HashAlg,
                        IdIut = SpecificationMapping.IutId,
                        IdServer = SpecificationMapping.ServerId,
                        IutKeyAgreementRole = group.KasRole,
                        IutKeyConfirmationRole = group.KcRole,
                        KasMode = group.KasMode,
                        KasValTestDisposition = testCaseDisposition,
                        KeyConfirmationDirection = group.KcType,
                        KeyLen = group.KeyLen,
                        MacLen = group.MacLen,
                        MacType = group.MacType,
                        OiPattern = group.OiPattern
                    }
                );

                var testCase = new TestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
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
                    IdIutLen = result.IdIutLen,
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
                    TestCaseDisposition = testCaseDisposition,
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