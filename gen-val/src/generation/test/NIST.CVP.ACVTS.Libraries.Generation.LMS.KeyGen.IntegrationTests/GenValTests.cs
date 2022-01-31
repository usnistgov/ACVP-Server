using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "LMS";
        public override string Mode { get; } = "KeyGen";


        public override AlgoMode AlgoMode => AlgoMode.LMS_KeyGen_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.pub != null)
            {
                testCase.pub = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.pub.ToString())).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new[]
            {
                new SpecificCapability
                {
                    Levels = new[]
                    {
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        }
                    }
                },

                new SpecificCapability
                {
                    Levels = new[]
                    {
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        },
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        }
                    }
                },

                new SpecificCapability
                {
                    Levels = new[]
                    {
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        },
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W2"
                        }
                    }
                },

                new SpecificCapability
                {
                    Levels = new[]
                    {
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        },
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H10",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        }
                    }
                },

                new SpecificCapability
                {
                    Levels = new[]
                    {
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        },
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        },
                        new LmsLevelParameters
                        {
                            LmsType = "LMS_SHA256_M32_H5",
                            LmotsType = "LMOTS_SHA256_N32_W4"
                        }
                    }
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Specific = true,
                SpecificCapabilities = caps
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new GeneralCapabilities
            {
                LmsTypes = new[] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10" },
                LmotsTypes = ParameterValidator.VALID_LMOTS_TYPES
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = caps
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
