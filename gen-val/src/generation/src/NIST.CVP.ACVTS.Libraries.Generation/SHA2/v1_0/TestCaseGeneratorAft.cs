using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _caseSizes;
        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var blockSize = ShaAttributes.GetShaAttributes(group.Function, group.DigestSize).blockSize;
            _caseSizes = DetermineMessageLength(group.MessageLength, blockSize);
            NumberOfTestCasesToGenerate = blockSize;

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = _caseSizes.Pop()
            };

            try
            {
                var oracleResult = await _oracle.GetShaCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ShuffleQueue<int> DetermineMessageLength(MathDomain messageLength, int blockSize)
        {
            var list = new List<int>();
            var minMax = messageLength.GetDomainMinMax();

            list.Add(minMax.Minimum);
            // below block size message lengths
            list.AddRangeIfNotNullOrEmpty(messageLength.GetRandomValues(x => (x < blockSize), blockSize / 3));
            // block size
            if (messageLength.IsWithinDomain(blockSize))
            {
                list.Add(blockSize);
            }
            // above block size message lengths
            list.AddRangeIfNotNullOrEmpty(messageLength.GetRandomValues(x => (x > blockSize), blockSize / 3));
            // message lengths that are greater than blocksize*x - 32 (for small block size) or 64 (for large block size)
            list.AddRangeIfNotNullOrEmpty(
                messageLength.GetRandomValues(x => (x > blockSize) && x % blockSize < blockSize / 16, blockSize / 3));

            // above block size message lengths 
            list.Add(minMax.Maximum);

            return new ShuffleQueue<int>(list);
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
