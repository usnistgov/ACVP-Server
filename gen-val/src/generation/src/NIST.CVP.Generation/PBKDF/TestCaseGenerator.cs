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

namespace NIST.CVP.Generation.PBKDF
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private List<int> _keyLens;
        private List<int> _passwordLens;
        private List<int> _saltLens;
        private List<int> _iterationCounts;

        public int NumberOfTestCasesToGenerate { get; private set; } = 50;

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
            _passwordLens = GetValuesFromDomain(group.PasswordLength);
            _saltLens = GetValuesFromDomain(group.SaltLength);
            _iterationCounts = GetVariedValuesFromDomain(group.IterationCount);
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var keyLen = _keyLens[caseNo % _keyLens.Count];
            var passLen = _passwordLens[caseNo % _passwordLens.Count];
            var saltLen = _saltLens[caseNo % _saltLens.Count];
            var itrCount = _iterationCounts[caseNo % _iterationCounts.Count];
            
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

        private List<int> GetVariedValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> {minMax.Minimum, minMax.Maximum};
            var smallValuesPulled = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum && v < 100000,
                NumberOfTestCasesToGenerate - 4, true);            
            var largeValuesPulled = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum && v >= 100000, 2, true);
            valuesSelected.AddRange(smallValuesPulled);
            valuesSelected.AddRange(largeValuesPulled);

            return valuesSelected.Shuffle();
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