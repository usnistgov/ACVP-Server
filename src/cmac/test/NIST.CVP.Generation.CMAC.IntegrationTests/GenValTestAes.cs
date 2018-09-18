using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "CMAC";
        public override string Mode { get; } = "AES";

        public override AlgoMode AlgoMode => AlgoMode.CMAC_AES;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
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
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Capabilities = new List<Capability>()
                {
                    new Capability()
                    {
                        KeyLen = 128,
                        Direction = "gen",
                        MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                        MacLen = new MathDomain()
                            .AddSegment(new ValueDomainSegment(128))
                            .AddSegment(new ValueDomainSegment(127)),
                    },
                    new Capability()
                    {
                        KeyLen = 128,
                        Direction = "ver",
                        MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                        MacLen = new MathDomain()
                            .AddSegment(new ValueDomainSegment(128))
                            .AddSegment(new ValueDomainSegment(127)),
                    },
                }.ToArray(),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();
            
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Capabilities = new List<Capability>()
                {
                    new Capability()
                    {
                        KeyLen = 128,
                        Direction = "gen",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                    new Capability()
                    {
                        KeyLen = 128,
                        Direction = "ver",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                    new Capability()
                    {
                        KeyLen = 192,
                        Direction = "gen",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                    new Capability()
                    {
                        KeyLen = 192,
                        Direction = "ver",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                    new Capability()
                    {
                        KeyLen = 256,
                        Direction = "gen",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                    new Capability()
                    {
                        KeyLen = 256,
                        Direction = "ver",
                        MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                        MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                    },
                }.ToArray(),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
