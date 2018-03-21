using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    public class GenValTestsShake : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHAKE";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.SHAKE;

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                Mode = Mode,
                DigestSizes = new[] { 128 },
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
            minMax.AddSegment(new RangeDomainSegment(null, 256, 4096, 1));

            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new[] { 128, 256 },
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
