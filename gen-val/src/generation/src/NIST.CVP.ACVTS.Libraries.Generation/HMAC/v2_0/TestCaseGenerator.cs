using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _keyLengths;
        private ShuffleQueue<int> _macLengths;
        private ShuffleQueue<int> _messageLengths;

        public int NumberOfTestCasesToGenerate => 150;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var blockSize = ShaAttributes.GetShaAttributes(group.ShaMode, group.ShaDigestSize).blockSize;
            
            // Differing algo logic depending on keyLen < blockSize, keyLen = blockSize, keyLen > blockSize
            // Ensure we grab values from each section of logic (where applicable)
            var keyLengths = new List<int>();
            
            keyLengths.AddRangeIfNotNullOrEmpty(group.KeyLen.GetDomainMinMaxAsEnumerable());
            keyLengths.AddRangeIfNotNullOrEmpty(group.KeyLen.GetRandomValues(x => x <  blockSize, 50));
            keyLengths.AddRangeIfNotNullOrEmpty(group.KeyLen.GetRandomValues(x => x == blockSize, 1));
            keyLengths.AddRangeIfNotNullOrEmpty(group.KeyLen.GetRandomValues(x => x >  blockSize, 50));
            keyLengths.AddRangeIfNotNullOrEmpty(group.KeyLen.GetRandomValues(_ => true, NumberOfTestCasesToGenerate - keyLengths.Count));    // Remaining cases random
            _keyLengths = new ShuffleQueue<int>(keyLengths);
            
            // Any random mac lengths with min/max included
            var macLens = new List<int>();

            macLens.AddRangeIfNotNullOrEmpty(group.MacLen.GetDomainMinMaxAsEnumerable());
            macLens.AddRangeIfNotNullOrEmpty(group.MacLen.GetRandomValues(_ => true, NumberOfTestCasesToGenerate - macLens.Count));
            _macLengths = new ShuffleQueue<int>(macLens);

            // Any random message lengths with min/max included
            var messageLens = new List<int>();
            
            messageLens.AddRangeIfNotNullOrEmpty(group.MessageLen.GetDomainMinMaxAsEnumerable());
            messageLens.AddRangeIfNotNullOrEmpty(group.MessageLen.GetRandomValues(_ => true, NumberOfTestCasesToGenerate - messageLens.Count));
            _messageLengths = new ShuffleQueue<int>(messageLens);
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new HmacParameters
            {
                KeyLength = _keyLengths.Pop(),
                MacLength = _macLengths.Pop(),
                MessageLength = _messageLengths.Pop(),
                ShaMode = group.ShaMode,
                ShaDigestSize = group.ShaDigestSize
            };

            try
            {
                var oracleResult = await _oracle.GetHmacCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    KeyLen = param.KeyLength,
                    Message = oracleResult.Message,
                    MessageLen = param.MessageLength,
                    Mac = oracleResult.Tag,
                    MacLen = param.MacLength
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
