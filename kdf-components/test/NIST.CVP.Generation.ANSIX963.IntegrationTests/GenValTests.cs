using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using KDFComponent;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ansix9.63";

        public override Executable Generator => Program.Main;
        public override Executable Validator => KDFComponent_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] {Mode};
            KDFComponent.AutofacConfig.OverrideRegistrations = null;
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            KDFComponent.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.keyData != null)
            {
                var bs = new BitString(testCase.keyData.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.keyData = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = new [] {"sha2-224"},
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new [] {224},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = new [] {"sha2-224", "sha2-256", "sha2-384"},
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new [] {224, 283},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new [] {224, 521},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
