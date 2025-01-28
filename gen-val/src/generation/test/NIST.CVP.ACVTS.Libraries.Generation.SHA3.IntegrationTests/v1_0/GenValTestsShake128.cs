using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v1_0
{
    public class GenValTestsShake128 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHAKE-128";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.SHAKE_128_v1_0;

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                DigestSizes = new List<int>() { 128 },
                BitOrientedInput = false,
                BitOrientedOutput = false,
                IncludeNull = false,
                OutputLength = minMax,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 128, 4096, 1));

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                DigestSizes = new List<int>() { 128 },
                BitOrientedInput = true,
                BitOrientedOutput = true,
                IncludeNull = true,
                OutputLength = minMax,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
