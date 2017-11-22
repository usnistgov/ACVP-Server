using System;
using System.IO;
using Autofac;
using KAS;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.KAS.FFC;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsFfc : GenValTestsBase
    {
        public override string Algorithm => "KAS-FFC";
        public override string Mode => string.Empty;

        public override Executable Generator => Program.Main;
        public override Executable Validator => KAS_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { Algorithm };

            AutofacConfig.OverrideRegistrations = null;
            KAS_Val.AutofacConfig.OverrideRegistrations = null;

            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<PqgProviderPreGenerated>().AsImplementedInterfaces();
            };
        }
        
        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            KAS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            KAS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a hashZIut, change it
            if (testCase.hashZIut != null)
            {
                BitString bs = new BitString(testCase.hashZIut.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.hashZIut = bs.ToHex();
            }
            // If TC has a tagIut, change it
            if (testCase.tagIut != null)
            {
                BitString bs = new BitString(testCase.tagIut.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tagIut = bs.ToHex();
            }
            // If TC has a result, change it
            if (testCase.result != null)
            {
                testCase.result = testCase.result.ToString().Equals("pass") ? "fail" : "pass";
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Function = new string[] { "dpGen" },
                Scheme = new Schemes()
                {
                    DhEphem = new DhEphem()
                    {
                        Role = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" }
                                }
                            }
                        }
                    },
                    Mqv1 = new Mqv1()
                    {
                        Role = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" }
                                }
                            }
                        }
                    }
                },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Function = new string[] { "dpGen", "dpVal", "keyPairGen", "fullVal", "keyRegen" },
                Scheme = new Schemes()
                {
                    DhEphem = new DhEphem()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-256", "SHA2-384", "SHA2-512" }
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc()
                        {
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D256 = new MacOptionHmacSha2_d256()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D384 = new MacOptionHmacSha2_d384()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D512 = new MacOptionHmacSha2_d512()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-256", "SHA2-384", "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D256 = new MacOptionHmacSha2_d256()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D384 = new MacOptionHmacSha2_d384()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D512 = new MacOptionHmacSha2_d512()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Mqv1 = new Mqv1()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224", "SHA2-512" }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-256", "SHA2-512" }
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc()
                        {
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D512 = new MacOptionHmacSha2_d512()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D384 = new MacOptionHmacSha2_d384()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringConverter(),
                    new DomainConverter()
                },
                Formatting = Formatting.Indented
            });
            string fileName = $"{targetFolder}\\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}