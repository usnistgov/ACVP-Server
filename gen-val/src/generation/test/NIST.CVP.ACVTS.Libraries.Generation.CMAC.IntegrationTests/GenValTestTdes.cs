using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsTdes : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "CMAC-TDES";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.CMAC_TDES_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.testPassed != null)
            {
                if (testCase.testPassed == true)
                {
                    testCase.testPassed = false;
                }
                else
                {
                    testCase.testPassed = true;
                }
            }

            // If TC has a mac, change it
            if (testCase.mac != null)
            {
                BitString bs = new BitString(testCase.mac.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.mac = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new List<Capability>
                {
                    new Capability
                    {
                        Direction = new [] {"gen", "ver"},
                        KeyingOption = new [] {1},
                        MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                        MacLen = new MathDomain()
                            .AddSegment(new ValueDomainSegment(64))
                    }
                }.ToArray(),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var random = new Random800_90();

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new List<Capability>
                {
                    new Capability
                    {
                        Direction = new [] {"gen", "ver"},
                        KeyingOption = new [] {1},
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 32, 64, 8)),
                    },
                    new Capability
                    {
                        Direction = new [] {"ver"},
                        KeyingOption = new [] {2},
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 32, 64, 8)),
                    },
                }.ToArray(),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
