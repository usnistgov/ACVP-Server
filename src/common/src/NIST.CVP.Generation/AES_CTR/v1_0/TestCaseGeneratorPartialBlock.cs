using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NIST.CVP.Common.ExtensionMethods;
using NLog;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestCaseGeneratorPartialBlock : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const int _casesPerSize = 5;
        private readonly List<int> _validSizes = new List<int>();

        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorPartialBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var tempSizes = group.PayloadLength.GetValues(ParameterValidator.MAXIMUM_DATA_LEN).ToList();
            foreach (var size in tempSizes)
            {
                _validSizes.Add(size, _casesPerSize);
            }
            
            NumberOfTestCasesToGenerate = _validSizes.Count;
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var payloadLen = _validSizes[caseNo];

            // This is a little hacky... but single block CTR is the same as OFB. So we can get past the awkward factory
            // TODO fix this up
            var param = new AesParameters
            {
                DataLength = payloadLen,
                KeyLength = group.KeyLength,
                Direction = group.Direction,
                Mode = BlockCipherModesOfOperation.Ofb
            };

            try
            {
                var result = await _oracle.GetAesCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    PayloadLength = payloadLen,
                    Key = result.Key,
                    IV = result.Iv,
                    PlainText = result.PlainText,
                    CipherText = result.CipherText
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
