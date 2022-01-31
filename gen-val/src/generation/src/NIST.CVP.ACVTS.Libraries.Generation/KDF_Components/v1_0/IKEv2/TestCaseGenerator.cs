using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new IkeV2KdfParameters
            {
                HashAlg = group.HashAlg,
                GirLength = group.GirLength,
                NRespLength = group.NRespLength,
                NInitLength = group.NInitLength,
                DerivedKeyingMaterialLength = group.DerivedKeyingMaterialLength
            };

            try
            {
                var result = await _oracle.GetIkeV2KdfCaseAsync(param);

                var testCase = new TestCase
                {
                    NResp = result.NResp,
                    NInit = result.NInit,
                    DerivedKeyingMaterial = result.DerivedKeyingMaterial,
                    DerivedKeyingMaterialChild = result.DerivedKeyingMaterialChild,
                    DerivedKeyingMaterialDh = result.DerivedKeyingMaterialDh,
                    Gir = result.Gir,
                    GirNew = result.GirNew,
                    SKeySeed = result.SKeySeed,
                    SKeySeedReKey = result.SKeySeedReKey,
                    SpiInit = result.SpiInit,
                    SpiResp = result.SpiResp
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
