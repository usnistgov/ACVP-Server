using System.Linq;
using CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes : GenValTestBase
    {
        protected override int ExecuteMainGenerator(string fileName)
        {
            return Program.Main(new string[] { "CMAC-AES", fileName });
        }

        protected override int ExecuteMainValidator(string targetFolder)
        {
            return CMAC_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames).Prepend("CMAC-AES").ToArray()
            );
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES-128",
                Direction = new[] { "gen", "ver" },
                KeyLen = new[] { 128 },
                MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                MacLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(127)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();
            
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES-256",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new[] { 256 },
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
