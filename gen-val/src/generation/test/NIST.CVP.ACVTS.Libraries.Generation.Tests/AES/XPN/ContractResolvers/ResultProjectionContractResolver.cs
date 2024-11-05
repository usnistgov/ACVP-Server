using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN.ContractResolvers
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class ResultsProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
        private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
        private readonly Projection _projection = Projection.Result;

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
        /// Only the groupId and tests should be present in the result file
        /// </summary>
        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "external", "external", true, true);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            Assert.That(newTg.Function, Is.Not.EqualTo(tg.Function), nameof(newTg.Function));
            Assert.That(newTg.KeyLength, Is.Not.EqualTo(tg.KeyLength), nameof(newTg.KeyLength));
            Assert.That(newTg.AadLength, Is.Not.EqualTo(tg.AadLength), nameof(newTg.AadLength));
            Assert.That(newTg.TagLength, Is.Not.EqualTo(tg.TagLength), nameof(newTg.TagLength));
            Assert.That(newTg.PayloadLength, Is.Not.EqualTo(tg.PayloadLength), nameof(newTg.PayloadLength));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText
        /// all other properties excluded
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="ivGen">IV generation (internal or external in relation to IUT)</param>
        /// <param name="saltGen">Salt generation (internal or external in relation to IUT)</param>
        /// <param name="deferred">Is this a deferred test? (Internal IV generation encrypt)</param>
        [Test]
        [TestCase("encrypt", "internal", "internal", true)]
        [TestCase("encrypt", "internal", "external", true)]
        [TestCase("encrypt", "external", "internal", true)]
        [TestCase("encrypt", "external", "external", false)]
        public void ShouldSerializeEncryptCaseProperties(string function, string ivGen, string saltGen, bool deferred)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, ivGen, saltGen, deferred, true);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            // not included in results file
            Assert.That(newTc.Key, Is.Not.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.PlainText, Is.Not.EqualTo(tc.PlainText), nameof(newTc.PlainText));

            if (ivGen == "internal")
            {
                Assert.That(newTc.CipherText, Is.Not.Null, nameof(newTc.CipherText));
                Assert.That(newTc.Tag, Is.Not.Null, nameof(newTc.Tag));
                Assert.That(newTc.IV, Is.Not.Null, nameof(newTc.IV));
            }
            else
            {
                Assert.That(newTc.CipherText, Is.Not.Null, nameof(newTc.CipherText));
                Assert.That(newTc.Tag, Is.Not.Null, nameof(newTc.Tag));
                Assert.That(newTc.IV, Is.Null, nameof(newTc.IV));
            }

            if (saltGen == "internal")
            {
                Assert.That(newTc.CipherText, Is.Not.Null, nameof(newTc.CipherText));
                Assert.That(newTc.Tag, Is.Not.Null, nameof(newTc.Tag));
                Assert.That(newTc.Salt, Is.Not.Null, nameof(newTc.Salt));
            }
            else
            {
                Assert.That(newTc.CipherText, Is.Not.Null, nameof(newTc.CipherText));
                Assert.That(newTc.Tag, Is.Not.Null, nameof(newTc.Tag));
                Assert.That(newTc.Salt, Is.Null, nameof(newTc.Salt));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        /// <summary>
        /// Decrypt test group should contain the plainText, results array (when mct)
        /// all other properties excluded
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testPassed">The testType</param>
        [Test]
        [TestCase("decrypt", true)]
        [TestCase("decrypt", false)]
        public void ShouldSerializeDecryptCaseProperties(string function, bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, "external", "external", false, testPassed);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (testPassed)
            {
                Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));
                Regex regexPass = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.That(regexPass.Matches(json).Count == 0, Is.True, nameof(regexPass));
            }
            else
            {
                Assert.That(newTc.TestPassed, Is.EqualTo(tc.TestPassed), nameof(newTc.TestPassed));

                Regex regexFail = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.That(regexFail.Matches(json).Count > 0, Is.True, nameof(regexFail));
            }

            // not included in results file
            Assert.That(newTc.IV, Is.Not.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.Salt, Is.Not.EqualTo(tc.Salt), nameof(newTc.Salt));
            Assert.That(newTc.Key, Is.Not.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.CipherText, Is.Not.EqualTo(tc.CipherText), nameof(newTc.CipherText));

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            if (testPassed)
            {
                Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
            }
            else
            {
                Assert.That(newTc.TestPassed, Is.EqualTo(tc.TestPassed), nameof(newTc.TestPassed));
                Assert.That(newTc.TestPassed, Is.False, nameof(newTc.TestPassed));
            }

        }
    }
}
