using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
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

            IkeV2KdfResult result = null;
            try
            {
                result = _oracle.GetIkeV2KdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
