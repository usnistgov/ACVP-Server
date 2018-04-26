using System;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IIkeV1 _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, IIkeV1 algo)
        {
            _random800_90 = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }

            var testCase = new TestCase
            {
                NInit = _random800_90.GetRandomBitString(group.NInitLength),
                NResp = _random800_90.GetRandomBitString(group.NRespLength),
                CkyInit = _random800_90.GetRandomBitString(64),
                CkyResp = _random800_90.GetRandomBitString(64),
                Gxy = _random800_90.GetRandomBitString(group.GxyLength),
            };

            if (group.AuthenticationMethod == AuthenticationMethods.Psk)
            {
                testCase.PreSharedKey = _random800_90.GetRandomBitString(group.PreSharedKeyLength);
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            IkeResult ikeResult = null;
            try
            {
                ikeResult = _algo.GenerateIke(testCase.NInit, testCase.NResp, testCase.Gxy, testCase.CkyInit,
                    testCase.CkyResp, testCase.PreSharedKey);
                if (!ikeResult.Success)
                {
                    ThisLogger.Warn(ikeResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ikeResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.SKeyId = ikeResult.SKeyId;
            testCase.SKeyIdD = ikeResult.SKeyIdD;
            testCase.SKeyIdA = ikeResult.SKeyIdA;
            testCase.SKeyIdE = ikeResult.SKeyIdE;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
