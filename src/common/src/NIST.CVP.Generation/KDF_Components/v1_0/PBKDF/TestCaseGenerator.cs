using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly int _keyLen;
        private readonly int _passLen;
        private readonly int _saltLen;
        private readonly int _itrCount;
        
        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGenerator(IOracle oracle, int keyLen, int passLen, int saltLen, int itrCount)
        {
            _oracle = oracle;
            _keyLen = keyLen;
            _passLen = passLen;
            _saltLen = saltLen;
            _itrCount = itrCount;
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new PbKdfParameters
            {
                HashAlg = group.HashAlg,
                KeyLen = _keyLen,
                PassLen = _passLen,
                SaltLen = _saltLen,
                ItrCount = _itrCount
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

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}