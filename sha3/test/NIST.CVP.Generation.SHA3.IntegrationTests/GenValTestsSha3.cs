using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    public class GenValTestsSha3 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.SHA3;

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                DigestSizes = new[] { 224 },
                BitOrientedInput = false,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                DigestSizes = new [] {224, 256, 384, 512},
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
