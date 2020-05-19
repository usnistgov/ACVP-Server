using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Generation.SHA3.v1_0;
using NIST.CVP.Math.Domain;


namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    public class GenValTestsShake : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHAKE-128";
        public override string Mode { get; } = string.Empty;

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
                DigestSizes = new List<int>() { 128, 256 },
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
