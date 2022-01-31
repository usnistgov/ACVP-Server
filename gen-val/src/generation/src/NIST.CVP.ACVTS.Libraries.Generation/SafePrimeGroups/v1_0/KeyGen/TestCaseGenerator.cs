using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            // when sample, need to actually generate a key on behalf of the IUT.
            if (isSample)
            {
                try
                {
                    var result = await _oracle.GetDsaKeyAsync(new DsaKeyParameters()
                    {
                        DomainParameters = SafePrimesFactory.GetDomainParameters(group.SafePrimeGroup)
                    });

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                    {
                        Key = result.Key
                    });
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
                }
            }

            // nothing to do when running !isSample.
            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
        }

        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();
    }
}
