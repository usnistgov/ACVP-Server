using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 50;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.TestCasesForGroup;

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new PbKdfParameters
            {
                HashAlg = group.HashAlg,
                KeyLen = group.KeyLength.Pop(),
                PassLen = group.PasswordLength.Pop(),
                SaltLen = group.SaltLength.Pop(),
                ItrCount = group.IterationCount.Pop()
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

        private (ShuffleQueue<int> iterationQueue, List<int> iterationCountList) GetVariedValuesFromDomain(MathDomain domain, bool isSample)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> { minMax.Minimum, minMax.Maximum };
            var smallValuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v < 100000,
                NumberOfTestCasesToGenerate - 5);
            valuesSelected.AddRange(smallValuesPulled);

            if (!isSample)
            {
                var largeValuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v >= 100000,
                    5);

                valuesSelected.AddRange(largeValuesPulled);
            }

            return (new ShuffleQueue<int>(valuesSelected), valuesSelected);
        }

        private ShuffleQueue<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> { minMax.Minimum, minMax.Maximum };
            var valuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum, NumberOfTestCasesToGenerate - 2);
            valuesSelected.AddRange(valuesPulled);

            return new ShuffleQueue<int>(valuesSelected);
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
