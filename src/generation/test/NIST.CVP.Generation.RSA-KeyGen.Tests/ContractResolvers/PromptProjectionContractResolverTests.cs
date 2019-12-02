using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Generation.RSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests.ContractResolvers
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class PromptProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
        private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
        private readonly Projection _projection = Projection.Prompt;

        private VectorSetSerializer<TestVectorSet, TestGroup, TestCase> _serializer;
        private VectorSetDeserializer<TestVectorSet, TestGroup, TestCase> _deserializer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _serializer =
                new VectorSetSerializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider,
                    _contractResolverFactory
                );
            _deserializer = 
                new VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider
                );
        }

        /// <summary>
        /// All group level properties are present in the prompt file
        /// </summary>
        [Test]
        [TestCase(PrimeGenFips186_4Modes.B32)]
        [TestCase(PrimeGenFips186_4Modes.B33)]
        [TestCase(PrimeGenFips186_4Modes.B34)]
        [TestCase(PrimeGenFips186_4Modes.B35)]
        [TestCase(PrimeGenFips186_4Modes.B36)]
        public void ShouldSerializeGroupProperties(PrimeGenFips186_4Modes primeGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(primeGenMode);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            // Always include
            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Modulo, newTg.Modulo, nameof(newTg.Modulo));
            Assert.AreEqual(tg.FixedPubExp, newTg.FixedPubExp, nameof(newTg.FixedPubExp));
            Assert.AreEqual(tg.PubExp, newTg.PubExp, nameof(newTg.PubExp));
            Assert.AreEqual(tg.InfoGeneratedByServer, newTg.InfoGeneratedByServer, nameof(newTg.InfoGeneratedByServer));
            Assert.AreEqual(tg.KeyFormat, newTg.KeyFormat, nameof(newTg.KeyFormat));
            Assert.AreEqual(tg.PrimeGenMode, newTg.PrimeGenMode, nameof(newTg.PrimeGenMode));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            
            // Provable
            if (primeGenMode == PrimeGenFips186_4Modes.B32 || primeGenMode == PrimeGenFips186_4Modes.B34 ||
                primeGenMode == PrimeGenFips186_4Modes.B35)
            {
                Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            }

            // Probable
            if (primeGenMode == PrimeGenFips186_4Modes.B33 || primeGenMode == PrimeGenFips186_4Modes.B35 ||
                primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.AreEqual(tg.PrimeTest, newTg.PrimeTest, nameof(newTg.PrimeTest));
            }
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        [Test]
        [TestCase(PrimeGenFips186_4Modes.B32)]
        [TestCase(PrimeGenFips186_4Modes.B33)]
        [TestCase(PrimeGenFips186_4Modes.B34)]
        [TestCase(PrimeGenFips186_4Modes.B35)]
        [TestCase(PrimeGenFips186_4Modes.B36)]
        public void ShouldSerializeCaseProperties(PrimeGenFips186_4Modes primeGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(primeGenMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            // Always include
            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));
            
            // Not probable
            if (primeGenMode != PrimeGenFips186_4Modes.B33)
            {
                Assert.AreEqual(tc.E, newTc.E, nameof(newTc.E));
            }

            // Provable
            if (primeGenMode == PrimeGenFips186_4Modes.B32 || primeGenMode == PrimeGenFips186_4Modes.B34 ||
                primeGenMode == PrimeGenFips186_4Modes.B35)
            {
                Assert.AreEqual(tc.Seed, newTc.Seed, nameof(newTc.Seed));
            }

            // With Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B34 || primeGenMode == PrimeGenFips186_4Modes.B35 ||
                primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.AreEqual(tc.Bitlens, newTc.Bitlens, nameof(newTc.Bitlens));
            }
            
            // Probable With Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B35 || primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.AreEqual(tc.XP, newTc.XP, nameof(newTc.XP));
                Assert.AreEqual(tc.XQ, newTc.XQ, nameof(newTc.XQ));
            }
            
            // Probable With Probable Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.AreEqual(tc.XP1, newTc.XP1, nameof(newTc.XP1));
                Assert.AreEqual(tc.XP2, newTc.XP2, nameof(newTc.XP2));
                
                Assert.AreEqual(tc.XQ1, newTc.XQ1, nameof(newTc.XQ1));
                Assert.AreEqual(tc.XQ2, newTc.XQ2, nameof(newTc.XQ2));
            }
            
            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
