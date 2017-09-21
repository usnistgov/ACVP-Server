using System;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorG : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IGGeneratorValidator _gGen;
        private readonly IPQGeneratorValidator _pqGen;
        private readonly IShaFactory _shaFactory = new ShaFactory();

        public int NumberOfTestCasesToGenerate { get { return 5; } }

        public TestCaseGeneratorG(IRandom800_90 rand, IGGeneratorValidator gGen, IPQGeneratorValidator pqGen = null)
        {
            _rand = rand;
            _gGen = gGen;

            if (pqGen == null)
            {
                var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d512);
                _pqGen = new ProbablePQGeneratorValidator(_shaFactory.GetShaInstance(hashFunction));
            }
            else
            {
                _pqGen = pqGen;
            }
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
