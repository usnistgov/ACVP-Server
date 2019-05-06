using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NIST.CVP.Math.Helpers;
using NLog;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class TestCaseGeneratorAFTHash : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private List<int> _caseSizes = new List<int>();
        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorAFTHash(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var blockSize = SHAEnumHelpers.DetermineBlockSize(group.DigestSize);
            _caseSizes = DetermineMessageLength(group.MessageLength, blockSize);
            NumberOfTestCasesToGenerate = _caseSizes.Count;

            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = _caseSizes[caseNo]
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

        private List<int> DetermineMessageLength(MathDomain messageLength, int blockSize)
        {
            var list = new List<int>();
            messageLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = messageLength.GetDomainMinMax();
            
            list.Add(minMax.Minimum);
            list.AddRange(SmallMessageLengths(messageLength, blockSize));
            list.AddRange(messageLength.GetValues(x => (x > blockSize), blockSize, false));
            list.Add(minMax.Maximum);

            return list;
        }

        private IEnumerable<int> SmallMessageLengths(MathDomain messageLength, int blockSize)
        {
            // Keep pulling small message sizes (under a block) until we have enough cases
            // We know that digest size is always in `messageLength`, so this will never infinite loop
            var smallList = new List<int>();
            do
            {
                for (var i = 0; i <= blockSize; i++)
                {
                    if (messageLength.IsWithinDomain(i))
                    {
                        smallList.Add(i);
                    }
                }
            } while (smallList.Count <= blockSize);

            return smallList;
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
