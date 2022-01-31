using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => 50;
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _zzLen;
        private ShuffleQueue<int> _keyLen;
        private ShuffleQueue<int> _otherInfoLen;
        private ShuffleQueue<int> _suppInfoLen;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var keyLengths = group.KeyLen.GetDeepCopy();
            keyLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var keyLenValues = keyLengths.GetDomainMinMaxAsEnumerable().ToList();

            // Outputs over the HashAlg.OutputLen will cause the loop to happen multiple times (and counters advance), so we want to make sure that exists when possible
            keyLenValues.AddRangeIfNotNullOrEmpty(keyLengths.GetValues(x => x <= group.HashAlg.OutputLen, 18, true));
            keyLenValues.AddRangeIfNotNullOrEmpty(keyLengths.GetValues(x => x > group.HashAlg.OutputLen, 30, true));
            _keyLen = new ShuffleQueue<int>(keyLenValues);

            var zzLengths = group.ZzLen.GetDeepCopy();
            zzLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var zzLenValues = zzLengths.GetDomainMinMaxAsEnumerable().ToList();

            // Any values are fine
            zzLenValues.AddRangeIfNotNullOrEmpty(zzLengths.GetValues(x => true, 48, true));
            _zzLen = new ShuffleQueue<int>(zzLenValues);

            if (group.KdfType == AnsiX942Types.Concat)
            {
                var otherLengths = group.OtherInfoLen.GetDeepCopy();
                otherLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
                var otherValues = otherLengths.GetDomainMinMaxAsEnumerable().ToList();

                // Any values are fine
                otherValues.AddRangeIfNotNullOrEmpty(otherLengths.GetValues(x => true, 48, true));
                _otherInfoLen = new ShuffleQueue<int>(otherValues);
            }
            else
            {
                var suppLengths = group.SuppInfoLen.GetDeepCopy();
                suppLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
                var suppValues = suppLengths.GetDomainMinMaxAsEnumerable().ToList();

                // Any values are fine
                suppValues.AddRangeIfNotNullOrEmpty(suppLengths.GetValues(x => true, 48, true));
                _suppInfoLen = new ShuffleQueue<int>(suppValues);
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AnsiX942Parameters
            {
                KdfMode = group.KdfType,
                Oid = group.Oid,
                HashAlg = group.HashAlg,
                KeyLen = _keyLen.Pop(),
                OtherInfoLen = group.KdfType == AnsiX942Types.Concat ? _otherInfoLen.Pop() : 0,
                SuppInfoLen = group.KdfType == AnsiX942Types.Der ? _suppInfoLen.Pop() : 0,
                ZzLen = _zzLen.Pop()
            };

            try
            {
                var result = await _oracle.GetAnsiX942KdfCaseAsync(param);

                var testCase = new TestCase
                {
                    Zz = result.Zz,
                    OtherInfo = result.OtherInfo,
                    PartyUInfo = result.PartyUInfo,
                    PartyVInfo = result.PartyVInfo,
                    SuppPubInfo = result.SuppPubInfo,
                    SuppPrivInfo = result.SuppPrivInfo,
                    DerivedKey = result.DerivedKey
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
