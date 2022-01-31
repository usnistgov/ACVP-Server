using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestCaseGeneratorLdt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; private set; }

        // 1GB is 2^30 bytes or 2^33 bits
        private const long _1GB = 8_589_934_592;
        private ShuffleQueue<int> _ldtSizes;

        private readonly IOracle _oracle;

        public TestCaseGeneratorLdt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.LargeDataSizes.Length;
            _ldtSizes = new ShuffleQueue<int>(group.LargeDataSizes.ToList());
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            // Easy starter values
            var fullLen = _ldtSizes.Pop() * _1GB;
            var contentLen = 64;    // this value is intentionally chosen because it maximizes the speed of our SHA implementation skipping most code branches (this applies to SHA2, not SHA3, but this is here for consistency)

            var param = new ShaLargeDataParameters
            {
                ExpansionMode = ExpansionMode.Repeating,
                FullLength = fullLen,
                HashFunction = group.CommonHashFunction,
                MessageLength = contentLen
            };

            try
            {
                var oracleResult = await _oracle.GetShaLdtCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    LargeMessage = oracleResult.MessageContent,
                    Digest = oracleResult.Digest
                });
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
