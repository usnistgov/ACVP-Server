﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IDsaEccFactory _eccFactory;
        private IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 12;

        public TestCaseGenerator(IRandom800_90 rand, IDsaEccFactory eccFactory, IEccCurveFactory curveFactory)
        {
            _rand = rand;
            _eccFactory = eccFactory;
            _curveFactory = curveFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            var reason = group.TestCaseExpectationProvider.GetRandomReason();

            var testCase = new TestCase
            {
                Reason = reason.GetReason(),
                TestPassed = reason.GetReason() == TestCaseExpectationEnum.None
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            EccKeyPairGenerateResult keyPairResult = null;
            try
            {
                _eccDsa = _eccFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                keyPairResult = _eccDsa.GenerateKeyPair(new EccDomainParameters(_curveFactory.GetCurve(group.Curve)));
                if (!keyPairResult.Success)
                {
                    ThisLogger.Warn($"Error generating key pair: {keyPairResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key pair: {keyPairResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating key pair: {ex.StackTrace}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating key pair: {ex.StackTrace}");
            }

            testCase.KeyPair = keyPairResult.KeyPair;

            // Modify test case
            return new TestCaseGenerateResponse<TestGroup, TestCase>(ModifyTestCase(testCase, _curveFactory.GetCurve(group.Curve)));
        }

        private TestCase ModifyTestCase(TestCase testCase, IEccCurve curve)
        {
            var modifiedTestCase = testCase;

            if (testCase.Reason == TestCaseExpectationEnum.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = testCase.KeyPair.PublicQ;

                do
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                modifiedTestCase.KeyPair = new EccKeyPair(modifiedPublicQ, testCase.KeyPair.PrivateD);
            }
            else if (testCase.Reason == TestCaseExpectationEnum.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = testCase.KeyPair.PublicQ;

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                modifiedTestCase.KeyPair = new EccKeyPair(modifiedPublicQ, testCase.KeyPair.PrivateD);
            }

            return modifiedTestCase;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}