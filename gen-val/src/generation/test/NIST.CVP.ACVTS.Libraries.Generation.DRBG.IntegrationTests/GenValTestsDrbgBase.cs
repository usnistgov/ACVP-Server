using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.DRBG.IntegrationTests
{
    public abstract class GenValTestsDrbgBase : GenValTestsSingleRunnerBase
    {
        public abstract override string Algorithm { get; }
        public override string Mode { get; } = null;

        public abstract string[] Modes { get; }
        public abstract int[] SeedLength { get; }


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a returnedBIts, change it
            if (testCase.returnedBits != null)
            {
                var bs = new BitString(testCase.returnedBits.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.returnedBits = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var index = 0;
            var otherIndex = 1;

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                ReseedImplemented = true,
                PredResistanceEnabled = new[] { true },
                Capabilities = new[]
                {
                    new Capability
                    {
                        Mode = Modes[index],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        AdditionalInputLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), SeedLength[index], SeedLength[index] + 64, 64)),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        ReturnedBitsLen = 256,
                        DerFuncEnabled = true
                    },
                    new Capability
                    {
                        Mode = Modes[otherIndex],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        ReturnedBitsLen = 256,
                        DerFuncEnabled = false
                    }
                }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var capabilities = new Capability[Modes.Length * 2];
            var bools = new[] { true, false };

            for (var i = 0; i < Modes.Length; i++)
            {
                for (var j = 0; j < bools.Length; j++)
                {
                    capabilities[i + j * Modes.Length] = new Capability
                    {
                        Mode = Modes[i],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                        ReturnedBitsLen = 4096,
                        DerFuncEnabled = bools[j]
                    };
                }
            }

            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                ReseedImplemented = true,
                PredResistanceEnabled = new[] { true, false },
                Capabilities = capabilities
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
