﻿using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.Hash256.ContractResolvers;

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
        var tvs = TestDataMother.GetTestGroups();
        var tg = tvs.TestGroups[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];

        Assert.Multiple(() =>
        {
            Assert.That(tg.TestGroupId, Is.EqualTo(newTg.TestGroupId));
            Assert.That(newTg.Tests, Has.Count.EqualTo(tg.Tests.Count), nameof(newTg.Tests.Count));
            Assert.That(tg.MessageLength, Is.Not.EqualTo(newTg.MessageLength), nameof(newTg.MessageLength));
        });
    }

    /// <summary>
    /// Only includes the test case Id and the returnedBits
    /// </summary>
    [Test]
    public void ShouldSerializeCaseProperties()
    {
        var tvs = TestDataMother.GetTestGroups();
        var tg = tvs.TestGroups[0];
        var tc = tg.Tests[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];
        var newTc = newTg.Tests[0];

        Assert.Multiple(() =>
        {
            Assert.That(tc.ParentGroup.TestGroupId, Is.EqualTo(newTg.TestGroupId));
            Assert.That(tc.TestCaseId, Is.EqualTo(newTc.TestCaseId));
            Assert.That(tc.Message, Is.Not.EqualTo(newTc.Message));
            Assert.That(tc.MessageBitLength, Is.Not.EqualTo(newTc.MessageBitLength));
            Assert.That(tc.Digest, Is.EqualTo(newTc.Digest));
        });

        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json), Is.Empty);
    }
}
