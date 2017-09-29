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
        public void ShouldHaveCorrectNumberOfSchemes()
        {
            /* 
             * TODO update this as more schemes are added.
             * Wanted to add a test that will fail once schemes are added to help 
             * ensure the other tests are updated to account for new schemes.
            */
            const int numberOfSchemesImplemented = 1;

            Assert.AreEqual(numberOfSchemesImplemented, SpecificationMapping.FfcSchemeMapping.Count);
        }

        private static object[] _testShouldGetEnumFromType = new object[]
        {
            new object[]
            {
                new DhEphem(),
                FfcScheme.DhEphem
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetEnumFromType))]
        public void ShouldGetEnumFromType(SchemeBase schemeBase, FfcScheme expectedSchemeEnum)
        {
            var result = SpecificationMapping.GetEnumFromType(schemeBase);

            Assert.AreEqual(expectedSchemeEnum, result);
        }

        private class FakeSchemeBase : SchemeBase { }

        [Test]
        public void ShouldThrowWhenInvalidScheme()
        {
            Assert.Throws(typeof(ArgumentException),
                () => SpecificationMapping.GetEnumFromType(new FakeSchemeBase()));
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