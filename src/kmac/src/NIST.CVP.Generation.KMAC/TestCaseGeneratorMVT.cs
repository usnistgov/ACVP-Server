using System;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorMvt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _capacity = 0;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorMvt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (_capacity == 0)
            {
                SetDomainRandomness(group.MacLengths);
                _capacity = group.DigestSize * 2;
            }

            var param = DetermineParameters(group.KeyLengths, group.MacLengths, group.MessageLength, group.HexCustomization, group.XOF);

            try
            {
                var oracleResult = _oracle.GetKmacCase(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    MacLength = oracleResult.Tag.BitLength,
                    MacVerified = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }

        // can only be called once
        private void SetDomainRandomness(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
        }

        private KmacParameters DetermineParameters(MathDomain keyLengths, MathDomain macLengths, int messageLength, bool hexCustomization, bool xof)
        {
            var keyLength = keyLengths.GetDomainMinMax().Minimum;
            //var customizationLength = 0;      customization is taken care of by oracle
            var macLen = macLengths.GetValues(1).ElementAt(0);    // assuming there is only one segment

            return new KmacParameters()
            {
                CouldFail = true,
                DigestSize = _capacity / 2,
                HexCustomization = hexCustomization,
                KeyLength = keyLength,
                MacLength = macLen,
                MessageLength = messageLength,
                XOF = xof
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
