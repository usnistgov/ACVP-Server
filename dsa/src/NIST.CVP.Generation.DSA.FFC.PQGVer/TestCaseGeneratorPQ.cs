using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorPQ : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IPQGeneratorValidator _pqGen;
        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IRandom800_90 rand, IPQGeneratorValidator pqGen)
        {
            _rand = rand;
            _pqGen = pqGen;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {

        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
