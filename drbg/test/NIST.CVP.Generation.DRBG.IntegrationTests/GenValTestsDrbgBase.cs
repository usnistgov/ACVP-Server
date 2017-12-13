using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using DRBG;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
{
    public abstract class GenValTestsDrbgBase : GenValTestsBase
    {
        public abstract override string Algorithm { get; }
        public override string Mode { get; } = "";

        public override Executable Generator => Program.Main;
        public override Executable Validator => DRBG_Val.Program.Main;

        public abstract string[] Modes { get; }
        public abstract int[] SeedLength { get; }

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            DRBG_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            DRBG_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            DRBG_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a plainText, change it
            if (testCase.returnedBits != null)
            {
                var bs = new BitString(testCase.returnedBits.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.returnedBits = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var index = 0;

            var nonceLen = new MathDomain();
            nonceLen.AddSegment(new ValueDomainSegment(SeedLength[index]));

            var additionalInputLen = new MathDomain();
            additionalInputLen.AddSegment(new RangeDomainSegment(new Random800_90(), SeedLength[index], SeedLength[index] + 64, 64));

            var persoStringLen = new MathDomain();
            persoStringLen.AddSegment(new ValueDomainSegment(SeedLength[index]));

            var entropyInputLen = new MathDomain();
            entropyInputLen.AddSegment(new ValueDomainSegment(SeedLength[index]));

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
                        NonceLen = nonceLen,
                        AdditionalInputLen = additionalInputLen,
                        PersoStringLen = persoStringLen,
                        EntropyInputLen = entropyInputLen,
                        ReturnedBitsLen = 128 * 4,
                        DerFuncEnabled = true
                    }
                }
            };

            return CreateRegistration(targetFolder, p);
        }

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
                        AdditionalInputLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), SeedLength[index], SeedLength[index] + 64, 64)),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[index])),
                        ReturnedBitsLen = 128 * 4,
                        DerFuncEnabled = true
                    },
                    new Capability
                    {
                        Mode = Modes[otherIndex],
                        NonceLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        AdditionalInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        PersoStringLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        EntropyInputLen = new MathDomain().AddSegment(new ValueDomainSegment(SeedLength[otherIndex])),
                        ReturnedBitsLen = 256 * 4,
                        DerFuncEnabled = false
                    }
                }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var capabilities = new Capability[Modes.Length * 2];
            var bools = new[] {true, false};

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
                        ReturnedBitsLen = 128 * 4,
                        DerFuncEnabled = bools[j]
                    };
                }
            }

            Parameters p = new Parameters
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
