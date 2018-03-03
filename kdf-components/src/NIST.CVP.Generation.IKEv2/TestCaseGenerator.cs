using System;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IIkeV2 _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, IIkeV2 algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var testCase = new TestCase
            {
                NInit = _rand.GetRandomBitString(group.NInitLength),
                NResp = _rand.GetRandomBitString(group.NRespLength),
                Gir = _rand.GetRandomBitString(group.GirLength),
                GirNew = _rand.GetRandomBitString(group.GirLength),
                SpiInit = _rand.GetRandomBitString(64),
                SpiResp = _rand.GetRandomBitString(64),
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            IkeResult ikeResult = null;
            try
            {
                ikeResult = _algo.GenerateIke(testCase.NInit, testCase.NResp, testCase.Gir, testCase.GirNew, testCase.SpiInit, testCase.SpiResp, group.DerivedKeyingMaterialLength);
                if (!ikeResult.Success)
                {
                    ThisLogger.Warn(ikeResult.ErrorMessage);
                    return new TestCaseGenerateResponse(ikeResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.SKeySeed = ikeResult.SKeySeed;
            testCase.DerivedKeyingMaterial = ikeResult.DKM;
            testCase.DerivedKeyingMaterialChild = ikeResult.DKMChildSA;
            testCase.DerivedKeyingMaterialDh = ikeResult.DKMChildSADh;
            testCase.SKeySeedReKey = ikeResult.SKeySeedReKey;

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
