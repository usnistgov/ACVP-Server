using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.HKDF.v1_0
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        
        private ShuffleQueue<int> _keyLens;
        private ShuffleQueue<int> _saltLens;
        private ShuffleQueue<int> _inputLens;
        private ShuffleQueue<int> _infoLens;

        public int NumberOfTestCasesToGenerate { get; set; } = 50;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 15;
            }
            
            _keyLens = GetValuesFromDomain(group.KeyLength);
            _inputLens = GetValuesFromDomain(group.InputKeyingMaterialLength);
            _saltLens = GetValuesFromDomain(group.SaltLength);
            _infoLens = GetValuesFromDomain(group.OtherInfoLength);
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var param = new HkdfParameters
            {
                HmacAlg = group.HmacAlg,
                KeyLen = _keyLens.Pop(),
                InfoLen = _infoLens.Pop(),
                SaltLen = _saltLens.Pop(),
                InputLen = _inputLens.Pop()
            };

            try
            {
                var result = await _oracle.GetHkdfCaseAsync(param);

                var testCase = new TestCase
                {
                    InputKeyingMaterial = result.InputKeyingMaterial,
                    Salt = result.Salt,
                    OtherInfo = result.OtherInfo,
                    DerivedKey = result.DerivedKey,
                    KeyLength = param.KeyLen
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ShuffleQueue<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> {minMax.Minimum, minMax.Maximum};
            var valuesPulled = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum, NumberOfTestCasesToGenerate - 2, true);            
            valuesSelected.AddRange(valuesPulled);

            return new ShuffleQueue<int>(valuesSelected);
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}