using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _currentTestCase = 0;
        private List<int> _keyLens;
        private List<int> _passwordLens;
        private List<int> _saltLens;
        private List<int> _iterationCounts;
        
        public int NumberOfTestCasesToGenerate => 50;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            // Set up lists of values
            if (_currentTestCase == 0)
            {
                _keyLens = GetValuesFromDomain(group.KeyLength);
                _passwordLens = GetValuesFromDomain(group.PasswordLength);
                _saltLens = GetValuesFromDomain(group.SaltLength);
                _iterationCounts = GetValuesFromDomain(group.IterationCount);
            }
            
            var keyLen = _keyLens[_currentTestCase % _keyLens.Count];
            var passLen = _passwordLens[_currentTestCase % _passwordLens.Count];
            var saltLen = _saltLens[_currentTestCase % _saltLens.Count];
            var itrCount = _iterationCounts[_currentTestCase % _iterationCounts.Count];

            _currentTestCase++;
            
            var param = new PbKdfParameters
            {
                HashAlg = group.HashAlg,
                KeyLen = keyLen,
                PassLen = passLen,
                SaltLen = saltLen,
                ItrCount = itrCount
            };

            try
            {
                var result = await _oracle.GetPbKdfCaseAsync(param);

                var testCase = new TestCase
                {
                    Password = result.Password,
                    Salt = result.Salt,
                    IterationCount = param.ItrCount,
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

        private List<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> {minMax.Minimum, minMax.Maximum};

            var valuesPulled = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum, NumberOfTestCasesToGenerate - 2, true);
            valuesSelected.AddRange(valuesPulled);

            return valuesSelected.Shuffle();
        }
        
        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}