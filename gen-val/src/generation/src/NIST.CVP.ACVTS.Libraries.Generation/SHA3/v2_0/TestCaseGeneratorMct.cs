using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;
using HashResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.HashResult;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public bool IsSample { get; set; } = false;
        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            IsSample = isSample;
            var param = new ShaParameters
            {
                HashFunction = group.HashFunction,
                MessageLength = ShaAttributes.GetShaAttributes(group.HashFunction.Mode, group.HashFunction.DigestSize).outputLen
            };

            try
            {
                if (group.MctVersion == MctVersions.Alternate)
                {
                    // Get the first between 128 and 1024
                    var values = group.MessageLengths.GetValues(x => x >= 128 && x <= 1024, 1, true);
                    
                    // If no values within the message domain that size, we search a wider array of numbers
                    if (!values.Any()) 
                    {
                        values = group.MessageLengths.GetValues(x => x >= 32 && x <= 4096, 1, true);
                    }

                    // If we got a value, set it here, if not, neither of those conditions were
                    // met above so we just get their max and use that
                    if (values.Any())
                    {
                        param.MessageLength = values.First();
                    }
                    else
                    {
                        param.MessageLength = group.MessageLengths.GetDomainMinMax().Maximum;
                    }
                    
                    param.IsAlternate = true;
                }

                var oracleResult = await _oracle.GetSha3MctCaseAsync(param);
                
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Seed.Message,
                    ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Message = element.Message, Digest = element.Digest })
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
