using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.ParallelHash.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.Tests.ContractResolvers
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
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.BitOrientedInput, newTg.BitOrientedInput, nameof(newTg.BitOrientedInput));
            Assert.AreEqual(tg.IncludeNull, newTg.IncludeNull, nameof(newTg.IncludeNull));
            Assert.AreEqual(tg.BitOrientedOutput, newTg.BitOrientedOutput, nameof(newTg.BitOrientedOutput));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("parallelhash", "aft", true)]
        [TestCase("parallelhash", "aft", false)]
        [TestCase("parallelhash", "mct", true)]
        [TestCase("parallelhash", "mct", false)]
        public void ShouldSerializeCaseProperties(string function, string testType, bool hexCustomization)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType, hexCustomization);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Message, newTc.Message, nameof(newTc.Message));
            Assert.AreEqual(tc.MessageLength, newTc.MessageLength, nameof(newTc.MessageLength));

            Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));

            Assert.AreNotEqual(tc.Digest, newTc.Digest, nameof(newTc.Digest));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            if (testType == "aft")
            {
                Assert.AreEqual(tc.BlockSize, newTc.BlockSize, nameof(newTc.BlockSize));
                Assert.AreEqual(tc.DigestLength, newTc.DigestLength, nameof(newTc.DigestLength));
                if (hexCustomization)
                {
                    Assert.AreEqual(tc.CustomizationHex, newTc.CustomizationHex, nameof(newTc.CustomizationHex));
                    Assert.AreNotEqual(tc.Customization, newTc.Customization, nameof(newTc.Customization));
                }
                else
                {
                    Assert.AreNotEqual(tc.CustomizationHex, newTc.CustomizationHex, nameof(newTc.CustomizationHex));
                    Assert.AreEqual(tc.Customization, newTc.Customization, nameof(newTc.Customization));
                }
            }
            else
            {
                Assert.AreNotEqual(tc.BlockSize, newTc.BlockSize, nameof(newTc.BlockSize));
                Assert.AreNotEqual(tc.DigestLength, newTc.DigestLength, nameof(newTc.DigestLength));
                Assert.AreNotEqual(tc.CustomizationHex, newTc.CustomizationHex, nameof(newTc.CustomizationHex));
                Assert.AreNotEqual(tc.Customization, newTc.Customization, nameof(newTc.Customization));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
