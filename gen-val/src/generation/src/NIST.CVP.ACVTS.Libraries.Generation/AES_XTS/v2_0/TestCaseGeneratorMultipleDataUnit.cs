using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class TestCaseGeneratorMultipleDataUnit : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<(int dataUnitLength, int payloadLength)> _payloadLen;

        public int NumberOfTestCasesToGenerate => 50;

        public TestCaseGeneratorMultipleDataUnit(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var iterations = 0;
            
            // Pick all the available payload length values
            var allPayloadLengths = new ShuffleQueue<int>(group.PayloadLen.GetSequentialValues(_ => true, 65536).ToList());
            
            var pairedLengths = new List<(int p, int du)>();
            
            do
            {
                // We need a dataUnitLength that provides (p % du) >= 128 so that the last data unit has at least one block of content. This is a gap in the XTS standard.
                var p = allPayloadLengths.Pop();
                var du = group.DataUnitLen.GetRandomValues(du => (du <= p) && (p % du >= 128), 1).FirstOrDefault();
                
                // Remove any cases where the dataUnitLengthToAdd is the default (i.e. where a valid test case could not be found)
                if (du != 0)
                {
                    pairedLengths.Add((p, du));
                }
                
                // 1) If we've exhausted the list of valid payload lengths, and have been unable to find any valid (payloadLen, dataUnitLen) pairs, abort.
                // Note: this condition should never be met due to checks performed by the ParameterValidator 
                if (iterations > allPayloadLengths.OriginalListCount && pairedLengths.Count == 0)
                {
                    throw new Exception("Unable to find test cases for XTS");
                }
                
                iterations++;
            }
                // 2) Otherwise, continue until we get 50 total test cases. 
            while (pairedLengths.Count < NumberOfTestCasesToGenerate);
            
            // Build ShuffleQueue of (payloadLength, dataUnitLength) tuples
            _payloadLen = new ShuffleQueue<(int payloadLength, int dataUnitLength)>(pairedLengths);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var (payloadLength, dataUnitLength) = _payloadLen.Pop();

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
