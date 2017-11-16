using NIST.CVP.Math;
using NUnit.Framework;
using System.Linq;
using AES_GCM;
using Autofac;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "GCM";

        public override Executable Generator => Program.Main;
        public override Executable Validator => AES_GCM_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = null;
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
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                BitString bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                BitString bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.plainText = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 0 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 0 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = new string[] { "encrypt", "decrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 128, 0 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new int[] { 128 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        //private string GetTestFileInternalIvNotSample(string targetFolder)
        //{
        //    Parameters p = new Parameters()
        //    {
        //        Algorithm = "AES-GCM",
        //        Direction = new string[] { "encrypt" },
        //        KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
        //        PtLen = new int[] { 128 },
        //        ivLen = new int[] { 96 },
        //        ivGen = ParameterValidator.VALID_IV_GEN[0],
        //        ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
        //        aadLen = new int[] { 128 },
        //        TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
        //        IsSample = false
        //    };

        //    return CreateRegistration(targetFolder, p);
        //}

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new int[] { 128, 120, 256 },
                ivLen = new int[] { 96, 128 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 128, 120 },
                TagLen = ParameterValidator.VALID_TAG_LENGTHS,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
