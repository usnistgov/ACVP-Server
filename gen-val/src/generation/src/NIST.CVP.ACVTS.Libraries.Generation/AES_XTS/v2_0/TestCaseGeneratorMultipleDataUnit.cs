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
    public class TestCaseGeneratorMultipleDataUnit : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<(int dataUnitLength, int payloadLength)> _payloadLen;

        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorMultipleDataUnit(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            // 4096 is a bit arbitrary, but should allow for multiple data units to exist within the payload.

            // Pick a bunch of low data unit length values
            var dataUnitDomain = group.DataUnitLen.GetDeepCopy();
            dataUnitDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var dataUnitLengths = dataUnitDomain.GetValues(x => x <= 1024, 50, true);

            // Pick a bunch of high payload length values
            var payloadLenDomain = group.PayloadLen.GetDeepCopy();
            payloadLenDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var payloadLengths = payloadLenDomain.GetValues(x => x > 1024, 50, true);

            var lengthTuple = dataUnitLengths.Zip(payloadLengths, (x, y) => (x, y)).ToList();
            _payloadLen = new ShuffleQueue<(int dataUnitLength, int payloadLength)>(lengthTuple);
            NumberOfTestCasesToGenerate = lengthTuple.Count;

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var (dataUnitLength, payloadLength) = _payloadLen.Pop();

            var param = new AesXtsParameters
            {
                Mode = BlockCipherModesOfOperation.Xts,
                KeyLength = group.KeyLen,
                Direction = EnumHelpers.GetEnumDescriptionFromEnum(group.Direction),
                TweakMode = EnumHelpers.GetEnumDescriptionFromEnum(group.TweakMode),
                DataLength = payloadLength,
                DataUnitLength = dataUnitLength
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
                    DataUnitLen = dataUnitLength
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
