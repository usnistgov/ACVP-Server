using NIST.CVP.Common;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    public class GenValTestsSha3 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.SHA3_v1_0;

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
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
                Revision = Revision,
                DigestSizes = new [] {224, 256},
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
