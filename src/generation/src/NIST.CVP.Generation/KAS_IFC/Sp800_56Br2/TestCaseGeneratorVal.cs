using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS_IFC.Sp800_56Br2
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly List<KasIfcValTestDisposition> _testDispositions;

        public TestCaseGeneratorVal(IOracle oracle, List<KasIfcValTestDisposition> validityTestCaseOptions)
        {
            _oracle = oracle;
            _testDispositions = validityTestCaseOptions;
            NumberOfTestCasesToGenerate = _testDispositions.Count;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var testCaseDisposition = TestCaseDispositionHelper.GetTestCaseIntention(_testDispositions);
            
            try
            {
                var result = await _oracle.GetKasValTestIfcAsync(new KasValParametersIfc()
                {
                    Disposition = testCaseDisposition,
                    L = group.L,
                    Modulo = group.Modulo,
                    Scheme = group.Scheme,
                    KasMode = group.KasMode,
                    KdfConfiguration = group.KdfConfiguration,
                    KtsConfiguration = group.KtsConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    PublicExponent = group.PublicExponent,
                    IutPartyId = group.IutId,
                    ServerPartyId = group.ServerId,
                    KeyConfirmationDirection = group.KeyConfirmationDirection,
                    KeyGenerationMethod = group.KeyGenerationMethod,
                    IutKeyAgreementRole = group.KasRole,
                    IutKeyConfirmationRole = group.KeyConfirmationRole
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = false,
                    TestPassed = result.TestPassed,
                    TestCaseDisposition = result.Disposition,
                    ServerZ = result.ServerZ,
                    ServerC = result.ServerC,
                    ServerNonce = result.ServerNonce,
                    ServerK = result.ServerK,
                    ServerKey = result.ServerKeyPair ?? new KeyPair() { PubKey = new PublicKey() },

                    IutZ = result.IutZ,
                    IutC = result.IutC,
                    IutNonce = result.IutNonce,
                    IutK = result.IutK,
                    IutKey = result.IutKeyPair ?? new KeyPair() { PubKey = new PublicKey() },
                    
                    KdfParameter = result.KdfParameter,
                    
                    MacKey = result.KasResult.MacKey,
                    MacData = result.KasResult.MacData,
                    
                    Dkm = result.KasResult.Dkm,
                    Tag = result.KasResult.Tag
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}