using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
{
    public abstract class GenValTestsHashBase : GenValTestsDrbgBase
    {
        public abstract override string Algorithm { get; }
        public abstract override string[] Modes { get; }
        public abstract override int[] SeedLength { get; }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var index = 0;
            var otherIndex = 1;
            
            var p = new Parameters
            {
                Algorithm = Algorithm,
                ReseedImplemented = true,
                PredResistanceEnabled = new []{true},
                Capabilities = new []
                {
                    new Capability
                    {
                        Mode = Modes[index],
                        // NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index] * 2)),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index] * 3)),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index] * 4)),
                        ReturnedBitsLen = SeedLength[index] * 4,
                    },
                    new Capability
                    {
                        Mode = Modes[otherIndex],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex] * 4)),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex] * 2)),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex] * 3)),
                        ReturnedBitsLen = SeedLength[otherIndex] * 8,
                    }
                }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var capabilities = new Capability[Modes.Length];

            for (var i = 0; i < Modes.Length; i++)
            {
                capabilities[i] = new Capability
                {
                    Mode = Modes[i],
                    NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i] * 3)),
                    PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i] * 4)),
                    EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i] * 5)),
                    ReturnedBitsLen = SeedLength[i] * 6,
                };
            }

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                ReseedImplemented = true,
                PredResistanceEnabled = new []{true, false},
                Capabilities = capabilities
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
