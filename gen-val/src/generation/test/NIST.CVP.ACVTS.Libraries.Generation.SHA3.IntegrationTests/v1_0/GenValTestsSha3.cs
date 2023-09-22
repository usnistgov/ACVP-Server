using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v1_0
{
    public class GenValTestsSha3 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3-224";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.SHA3_224_v1_0;

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                DigestSizes = new List<int>() { 224 },
                BitOrientedInput = false,
                IncludeNull = true,
                PerformLargeDataTest = new[] { 1, 2 },
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {   
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 16, 65536));
            
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                DigestSizes = new List<int>() { 224 },
                BitOrientedInput = true,
                IncludeNull = true,
                PerformLargeDataTest = ParameterValidator.VALID_LARGE_DATA_SIZES,
                IsSample = true,
                OutputLength = domain
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
