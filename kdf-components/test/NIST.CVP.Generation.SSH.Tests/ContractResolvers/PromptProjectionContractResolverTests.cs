using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.SSH.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SSH.Tests.ContractResolvers
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

        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Cipher, newTg.Cipher, nameof(newTg.Cipher));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
        }

        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.K, newTc.K, nameof(newTc.K));
            Assert.AreEqual(tc.H, newTc.H, nameof(newTc.H));
            Assert.AreEqual(tc.SessionId, newTc.SessionId, nameof(newTc.SessionId));

            Assert.AreNotEqual(tc.InitialIvClient, newTc.InitialIvClient, nameof(newTc.InitialIvClient));
            Assert.AreNotEqual(tc.EncryptionKeyClient, newTc.EncryptionKeyClient, nameof(newTc.EncryptionKeyClient));
            Assert.AreNotEqual(tc.IntegrityKeyClient, newTc.IntegrityKeyClient, nameof(newTc.IntegrityKeyClient));
            Assert.AreNotEqual(tc.InitialIvServer, newTc.InitialIvServer, nameof(newTc.InitialIvServer));
            Assert.AreNotEqual(tc.EncryptionKeyServer, newTc.EncryptionKeyServer, nameof(newTc.EncryptionKeyServer));
            Assert.AreNotEqual(tc.IntegrityKeyServer, newTc.IntegrityKeyServer, nameof(newTc.IntegrityKeyServer));
            
            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
