using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class TestCaseGeneratorSingleDataUnit : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _payloadLen;

        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorSingleDataUnit(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var payloadLenDomain = group.PayloadLen.GetDeepCopy();
            payloadLenDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var payloadLenValues = payloadLenDomain.GetDomainMinMaxAsEnumerable().ToList();
            payloadLenValues.AddRange(payloadLenDomain.GetValues(x => x <= 1024, 24, true));
            payloadLenValues.AddRange(payloadLenDomain.GetValues(x => x > 1024, 24, true));

            _payloadLen = new ShuffleQueue<int>(payloadLenValues);
            NumberOfTestCasesToGenerate = payloadLenValues.Count;

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var dataLength = _payloadLen.Pop();

            var param = new AesXtsParameters
            {
                Mode = BlockCipherModesOfOperation.Xts,
                KeyLength = group.KeyLen,
                Direction = EnumHelpers.GetEnumDescriptionFromEnum(group.Direction),
                TweakMode = EnumHelpers.GetEnumDescriptionFromEnum(group.TweakMode),
                DataLength = dataLength,
                DataUnitLength = dataLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesXtsCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    CipherText = oracleResult.CipherText,
                    I = oracleResult.Iv,
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    SequenceNumber = oracleResult.SequenceNumber,
                    XtsKey = new XtsKey(oracleResult.Key),
                    DataUnitLen = dataLength
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
