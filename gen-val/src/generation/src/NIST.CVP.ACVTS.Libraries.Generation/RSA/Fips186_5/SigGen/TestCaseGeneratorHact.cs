using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
{
    public class TestCaseGeneratorHact : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _dataLengths;
        private IDictionary<int, string> _jsonFiles = new Dictionary<int, string> { { 2048, "RsaSigGen2048.json" } };

        public TestCaseGeneratorHact(IOracle oracle)
        {
            _oracle = oracle;
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
        public int NumberOfTestCasesToGenerate { get; set; }

        // This should only be used with modulo == 2048, so the dictionary look-up is fine
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var hactObject = HactJsonFactory.GetHactParameters(group.Modulo);

            var lengths = hactObject.EncryptParameters.Select(ep => ep.MsgLen).ToList();
            _dataLengths = new ShuffleQueue<int>(lengths);
            NumberOfTestCasesToGenerate = _dataLengths.OriginalListCount;

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new RsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                Key = group.Key,
                Modulo = group.Modulo,
                PaddingScheme = group.Mode,
                SaltLength = group.SaltLen,
                MaskFunction = group.MaskFunction,
                MessageLength = _dataLengths.Pop()
            };

            try
            {
                RsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetRsaSignatureAsync(param);
                }
                else
                {
                    result = await _oracle.GetDeferredRsaSignatureAsync(param);
                }

                var testCase = new TestCase
                {
                    Message = result.Message,
                    Signature = result.Signature?.PadToModulusMsb(group.Modulo),
                    Salt = result.Salt,
                    Deferred = !isSample
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
    }
}
