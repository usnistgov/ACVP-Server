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
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        ReturnedBitsLen = SeedLength[index] * 4,
                    },
                    new Capability
                    {
                        Mode = Modes[otherIndex],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
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
                    NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                    AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                    PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                    EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[i])),
                    ReturnedBitsLen = SeedLength[i] * 4,
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
