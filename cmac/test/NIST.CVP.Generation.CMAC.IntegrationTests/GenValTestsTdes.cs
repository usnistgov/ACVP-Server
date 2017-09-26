using System.Linq;
using CMAC;
using NIST.CVP.Generation.CMAC.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsTdes : GenValTestBase
    {

        protected override int ExecuteMainGenerator(string fileName)
        {
            return Program.Main(new string[] { "CMAC-TDES", fileName });
        }

        protected override int ExecuteMainValidator(string targetFolder)
        {
            return CMAC_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames).Prepend("CMAC-TDES").ToArray()
            );
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-TDES",
                Direction = new [] { "gen", "ver" },
                KeyingOption = new[] {1},
                MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                MacLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
        
        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();
            
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-TDES",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                MacLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = false,
                KeyingOption = new []{1, 2}
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
