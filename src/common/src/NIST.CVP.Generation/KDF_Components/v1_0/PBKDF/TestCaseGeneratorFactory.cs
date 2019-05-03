using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private const int NumTestCases = 100; // From the TestCaseGenerator
        private int _currentTestCase = 0;
        private List<int> _keyLens;
        private List<int> _passwordLens;
        private List<int> _saltLens;
        private List<int> _iterationCounts;
        
        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (_currentTestCase == 0)
            {
                _keyLens = GetValuesFromDomain(testGroup.KeyLength);
                _passwordLens = GetValuesFromDomain(testGroup.PasswordLength);
                _saltLens = GetValuesFromDomain(testGroup.SaltLength);
                _iterationCounts = GetValuesFromDomain(testGroup.IterationCount);
            }

            var keyLen = _keyLens[_currentTestCase % _keyLens.Count];
            var passLen = _passwordLens[_currentTestCase % _passwordLens.Count];
            var saltLen = _saltLens[_currentTestCase % _saltLens.Count];
            var itrCount = _iterationCounts[_currentTestCase % _iterationCounts.Count];
            
            var testCaseGenerator = new TestCaseGenerator(_oracle, keyLen, passLen, saltLen, itrCount);
            _currentTestCase++;
            
            return testCaseGenerator;
        }

        private List<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            var valuesPulled = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum, NumTestCases - 2, true);
            valuesSelected = valuesSelected.Union(valuesPulled).ToList();

            return valuesSelected.Shuffle();
        }
    }
}