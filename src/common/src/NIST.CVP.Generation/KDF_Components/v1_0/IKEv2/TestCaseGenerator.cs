using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.IKEv2
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

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
