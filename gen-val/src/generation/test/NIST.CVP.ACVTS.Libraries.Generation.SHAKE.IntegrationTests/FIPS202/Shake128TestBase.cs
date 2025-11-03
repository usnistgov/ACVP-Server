using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.IntegrationTests.FIPS202
{
    public abstract class Shake128TestBase : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode { get; }
        public override string Algorithm { get; }
        public override string Mode { get; } = null;
        
        private Random800_90 _rand = new Random800_90();
        
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        
        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHAKE-128",
                Revision = Revision,
                Mode = Mode,
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(_rand, 8, 65528, 8)),
                OutputLength =  new MathDomain().AddSegment(new RangeDomainSegment(_rand, 256, 512, 8)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
        
        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHAKE-128",
                Revision = Revision,
                Mode = Mode,
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(_rand, 8, 65528, 8)),
                OutputLength =  new MathDomain().AddSegment(new RangeDomainSegment(_rand, 256, 512, 8)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
        
        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.md != null)
            {
                var bs = new BitString(testCase.md.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.md = bs.ToHex();
            }

            if (testCase.msg != null)
            {
                var bs = new BitString(testCase.msg.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.msg = bs.ToHex();
            }

            if (testCase.resultsArray != null)
            {
                var temp = testCase.resultsArray;
                var bsDigest = new BitString(testCase.resultsArray[0].md.ToString());
                bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                testCase.resultsArray[0].md = bsDigest.ToHex();
            }
        }
    }
}
