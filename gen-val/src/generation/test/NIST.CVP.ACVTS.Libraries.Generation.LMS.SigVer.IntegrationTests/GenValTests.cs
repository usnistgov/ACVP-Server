using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Tests;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "LMS";
        public override string Mode { get; } = "SigVer";


        public override AlgoMode AlgoMode => AlgoMode.LMS_SigVer_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
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
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new SpecificCapability[]
            {
                new SpecificCapability
                {
                    Levels = new LmsLevelParameters[]
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
                    Levels = new LmsLevelParameters[]
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
                    Levels = new LmsLevelParameters[]
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
                    Levels = new LmsLevelParameters[]
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
                    Levels = new LmsLevelParameters[]
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
                LmsTypes = new string[] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10" },
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
