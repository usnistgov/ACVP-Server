﻿using System;
using NIST.CVP.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SNMP
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ISnmp _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, ISnmp algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var testCase = new TestCase
            {
                Password = _rand.GetRandomAlphaCharacters(group.PasswordLength)
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SnmpResult snmpResult = null;
            try
            {
                snmpResult = _algo.KeyLocalizationFunction(group.EngineId, testCase.Password);
                if (!snmpResult.Success)
                {
                    ThisLogger.Warn(snmpResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(snmpResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.SharedKey = snmpResult.SharedKey;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}