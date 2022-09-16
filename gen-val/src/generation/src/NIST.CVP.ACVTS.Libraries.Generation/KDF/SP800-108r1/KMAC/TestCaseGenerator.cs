using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _derivedKeyLength;
        private ShuffleQueue<int> _keyDerivationKeyLength;
        private ShuffleQueue<int> _contextLength;
        private ShuffleQueue<int> _labelLength;

        public int NumberOfTestCasesToGenerate { get; } = 50;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            _derivedKeyLength = new ShuffleQueue<int>(GetRandomValuesWithMinMaxFromDomain(group.DerivedKeyLength, NumberOfTestCasesToGenerate));
            _keyDerivationKeyLength = new ShuffleQueue<int>(GetRandomValuesWithMinMaxFromDomain(group.KeyDerivationKeyLength, NumberOfTestCasesToGenerate));
            _contextLength = new ShuffleQueue<int>(GetRandomValuesWithMinMaxFromDomain(group.ContextLength, NumberOfTestCasesToGenerate));

            // If there is no LabelLength provided, it is 0 for all cases
            if (group.LabelLength == null)
            {
                _labelLength = new ShuffleQueue<int>(new List<int> { 0 });
            }
            else
            {
                _labelLength = new ShuffleQueue<int>(GetRandomValuesWithMinMaxFromDomain(group.LabelLength, NumberOfTestCasesToGenerate));
            }

            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new KdfKmacParameters
            {
                DerivedKeyLength = _derivedKeyLength.Pop(),
                KeyDerivationKeyLength = _keyDerivationKeyLength.Pop(),
                ContextLength = _contextLength.Pop(),
                LabelLength = _labelLength.Pop(),
                MacMode = group.MacMode
            };

            try
            {
                var oracleResult = await _oracle.GetKdfKmacCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    KeyDerivationKey = oracleResult.KeyDerivationKey,
                    DerivedKey = oracleResult.DerivedKey,
                    Context = oracleResult.Context,
                    Label = oracleResult.Label
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private List<int> GetRandomValuesWithMinMaxFromDomain(MathDomain domain, int totalValues)
        {
            var domainParameters = domain.GetDeepCopy();
            domainParameters.SetRangeOptions(RangeDomainSegmentOptions.Random);
            
            var domainOptions = new List<int>
            {
                domainParameters.GetDomainMinMax().Minimum,
                domainParameters.GetDomainMinMax().Maximum
            };
            
            domainOptions.AddRange(domainParameters.GetValues(_ => true, totalValues - 2, true));

            return domainOptions;
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
