using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class SpecificationMappingTests
    {
        #region GetHmacInfoFromParameterClass

        private static object[] _testShouldReturnHmacInfoCorrectly = new object[]
        {
            new object[]
            {
                new MacOptionHmacSha2_d224(),
                "HMAC-SHA2-244"
            },
            new object[]
            {
                new MacOptionHmacSha2_d256(),
                "HMAC-SHA2-256"
            },
            new object[]
            {
                new MacOptionHmacSha2_d384(),
                "HMAC-SHA2-384"
            },
            new object[]
            {
                new MacOptionHmacSha2_d512(),
                "HMAC-SHA2-512"
            },
        };

        [Test]
        [TestCaseSource(nameof(_testShouldReturnHmacInfoCorrectly))]
        public void ShouldReturnHmacInfoCorrectly(MacOptionsBase macOptionBase, string expectedSpecificationString)
        {
            var result = SpecificationMapping.GetHmacInfoFromParameterClass(macOptionBase);

            Assert.AreEqual(expectedSpecificationString, result.specificationHmac);
        }

        [Test]
        public void ShouldThrowWhenInvalidMacBaseProvided()
        {
            Assert.Throws(typeof(ArgumentException),
                () => SpecificationMapping.GetHmacInfoFromParameterClass(new MacOptionAesCcm()));
        }
        #endregion GetHmacInfoFromParameterClass

        #region GetMacInfoFromParameterClass
        private static object[] _testShouldReturnMacInfoCorrectly = new object[]
        {
            new object[]
            {
                new MacOptionAesCcm(), 
                "AES-CCM"
            },
            new object[]
            {
                new MacOptionCmac(),
                "CMAC"
            },
            new object[]
            {
                new MacOptionHmacSha2_d224(),
                "HMAC-SHA2-244"
            },
            new object[]
            {
                new MacOptionHmacSha2_d256(),
                "HMAC-SHA2-256"
            },
            new object[]
            {
                new MacOptionHmacSha2_d384(),
                "HMAC-SHA2-384"
            },
            new object[]
            {
                new MacOptionHmacSha2_d512(),
                "HMAC-SHA2-512"
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldReturnMacInfoCorrectly))]
        public void ShouldReturnMacInfoCorrectly(MacOptionsBase macOptionBase, string expectedSpecificationString)
        {
            var result = SpecificationMapping.GetMacInfoFromParameterClass(macOptionBase);

            Assert.AreEqual(expectedSpecificationString, result.specificationMac);
        }

        private class FakeMacOptionBase : MacOptionsBase { }

        [Test]
        public void ShouldThrowWhenInvalidMacBaseProvidedToMac()
        {
            Assert.Throws(typeof(ArgumentException),
                () => SpecificationMapping.GetHmacInfoFromParameterClass(new FakeMacOptionBase()));
        }
        #endregion GetMacInfoFromParameterClass

        #region scheme

        [Test]
        public void ShouldHaveCorrectNumberOfSchemesFfc()
        {
            const int numberOfSchemesImplemented = 7;

            Assert.AreEqual(numberOfSchemesImplemented, SpecificationMapping.FfcSchemeMapping.Count);
        }


        [Test]
        public void ShouldHaveCorrectNumberOfSchemesEcc()
        {
            const int numberOfSchemesImplemented = 7;

            Assert.AreEqual(numberOfSchemesImplemented, SpecificationMapping.EccSchemeMapping.Count);
        }

        private static object[] _testShouldGetEnumFromTypeFfc = new object[]
        {
            new object[]
            {
                new FfcDhEphem(),
                FfcScheme.DhEphem
            },
            new object[]
            {
                new FfcMqv1(),
                FfcScheme.Mqv1
            },
            new object[]
            {
                new FfcDhHybrid1(),
                FfcScheme.DhHybrid1
            },
            new object[]
            {
                new FfcDhHybridOneFlow(),
                FfcScheme.DhHybridOneFlow
            },
            new object[]
            {
                new FfcDhOneFlow(),
                FfcScheme.DhOneFlow
            },
            new object[]
            {
                new FfcMqv2(),
                FfcScheme.Mqv2
            },
            new object[]
            {
                new FfcDhStatic(),
                FfcScheme.DhStatic
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetEnumFromTypeFfc))]
        public void ShouldGetEnumFromTypeFfc(SchemeBase schemeBase, FfcScheme expectedSchemeEnum)
        {
            var result = SpecificationMapping.GetFfcEnumFromType(schemeBase);

            Assert.AreEqual(expectedSchemeEnum, result);
        }

        private static object[] _testShouldGetEnumFromTypeEcc = new object[]
        {
            new object[]
            {
                new EccEphemeralUnified(), 
                EccScheme.EphemeralUnified
            },
            new object[]
            {
                new EccOnePassMqv(),
                EccScheme.OnePassMqv
            },
            new object[]
            {
                new EccFullMqv(),
                EccScheme.FullMqv
            },
            new object[]
            {
                new EccFullUnified(),
                EccScheme.FullUnified
            },
            new object[]
            {
                new EccOnePassDh(),
                EccScheme.OnePassDh
            },
            new object[]
            {
                new EccOnePassUnified(),
                EccScheme.OnePassUnified
            },
            new object[]
            {
                new EccStaticUnified(),
                EccScheme.StaticUnified
            },
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetEnumFromTypeEcc))]
        public void ShouldGetEnumFromType(SchemeBase schemeBase, EccScheme expectedSchemeEnum)
        {
            var result = SpecificationMapping.GetEccEnumFromType(schemeBase);

            Assert.AreEqual(expectedSchemeEnum, result);
        }

        private class FakeSchemeBase : SchemeBase { }

        [Test]
        public void ShouldThrowWhenInvalidSchemeFfc()
        {
            Assert.Throws(typeof(ArgumentException),
                () => SpecificationMapping.GetFfcEnumFromType(new FakeSchemeBase()));
        }

        [Test]
        public void ShouldThrowWhenInvalidSchemeEcc()
        {
            Assert.Throws(typeof(ArgumentException),
                () => SpecificationMapping.GetEccEnumFromType(new FakeSchemeBase()));
        }
        #endregion scheme

        #region FunctionArrayToFlags
        [Test]
        [TestCase(KasAssurance.None, new string[] { })]
        [TestCase(KasAssurance.DpGen, new string[] { "dpGen" })]
        [TestCase(KasAssurance.DpGen | KasAssurance.DpVal, new string[]{ "dpGen", "dpVal" })]
        public void ShouldParseCorrectly(KasAssurance expectedParse, string[] array)
        {
            var result = SpecificationMapping.FunctionArrayToFlags(array);

            Assert.AreEqual(expectedParse, result);
        }
        #endregion FunctionArrayToFlags
    }
}