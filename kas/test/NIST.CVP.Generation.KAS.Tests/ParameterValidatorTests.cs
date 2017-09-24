using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KAS.Tests.Builders;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private ParameterValidator _subject;
        private ParameterBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _subject = new ParameterValidator();
            _builder = new ParameterBuilder();
        }

        [Test]
        public void ShouldSucceedWithBaseBuilder()
        {
            var result = _subject.Validate(_builder.Build());

            Assert.IsTrue(result.Success);
        }

        private static object[] _testParameters = new object[]
        {
            new object[]
            {
                "null algorithm",
                new ParameterBuilder().WithAlgorithm(null).Build(),
                "algorithm"
            },
            new object[]
            {
                "invalid algorithm",
                new ParameterBuilder().WithAlgorithm("invalid").Build(),
                "algorithm"
            },
            new object[]
            {
                "null function",
                new ParameterBuilder().WithFunctions(null).Build(),
                "function"
            },
            new object[]
            {
                "invalid function",
                new ParameterBuilder().WithFunctions(new string[] { "invalid" }).Build(),
                "function"
            },
            new object[]
            {
                "null schemes",
                new ParameterBuilder().WithSchemes(null).Build(),
                "scheme"
            },
            new object[]
            {
                // TODO update with rest of schemes as they're added to ensure there are zero valid schemes provided
                "No valid schemes",
                new ParameterBuilder()
                .WithSchemes(
                    new SchemesBuilder().WithDhEphem(null).Build()
                ).Build(),
                "scheme"
            },
            new object[]
            {
                "null role dhEphem",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder().WithRole(null).BuildDhEphem()
                        ).Build()
                    ).Build(),
                "role"
            },
            new object[]
            {
                "invalid role dhEphem",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder().WithRole(new string[] { "invalid" }).BuildDhEphem()
                        ).Build()
                    ).Build(),
                "role"
            },
            new object[]
            {
                "no kas modes",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithNoKdfNoKc(null)
                                .WithKdfNoKc(null)
                                .WithKdfKc(null)
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "kas mode"
            },
            new object[]
            {
                "KC not valid for dhEphem",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfKc(
                                    new KdfKcBuilder().Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "key confirmation"
            },
            new object[]
            {
                "noKdfNoKc w/o parameterSet",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithNoKdfNoKc(
                                    new NoKdfNoKcBuilder().WithParameterSets(null).Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "parameterset"
            },
            new object[]
            {
                "kdfNoKc w/o parameterSet",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder().WithParameterSets(null).Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "parameterset"
            },
            new object[]
            {
                "kdfNoKc w/o kdfOptions",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder().WithKdfOptions(null).Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "kdfOption"
            },
            new object[]
            {
                "kdfNoKc w/ invalid oi element",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithKdfOptions(
                                            new KdfOptionsBuilder().WithAsn1("invalid||uPartyInfo").Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "oiPattern"
            },
            new object[]
            {
                "kdfNoKc w/ invalid oi literal",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithKdfOptions(
                                            new KdfOptionsBuilder().WithAsn1("literal[nonHex]||uPartyInfo").Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "oiPattern"
            },
            new object[]
            {
                "invalid hash",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithNoKdfNoKc(
                                    new NoKdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(false)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] { "invalid" })
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "hashAlgs"
            },
            new object[]
            {
                "hashLength below minimum",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithNoKdfNoKc(
                                    new NoKdfNoKcBuilder()
                                    .WithParameterSets(
                                        new ParameterSetBuilder(false)
                                        .WithFc(
                                            new ParameterSetBaseBuilder(false)
                                                .WithHashAlg(new string[] { "sha2-224"})
                                                .BuildFc()
                                        )
                                        .Build()
                                    )
                                    .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "hash length"
            },
            new object[]
            {
                "macOptions provided w/o KDF",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithNoKdfNoKc(
                                    new NoKdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true).Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "macOption"
            },
            new object[]
            {
                "no macOptions provided w/ KDF",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(false).Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "macOption"
            },
            new object[]
            {
                "no valid mac options",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(false)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(false)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "macOption is required"
            },
            new object[]
            {
                "aesCcm w/o nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(false)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(false)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(0)
                                                                        .BuildAesCcm()
                                                                )
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "AES-CCM Nonce Length"
            },
            new object[]
            {
                "invalid aesCcm nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(false)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(false)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildAesCcm()
                                                                )
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "AES-CCM Nonce Length"
            },
            new object[]
            {
                "cmac w/ nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildAesCmac()
                                                                )
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "unexpected for macOptions"
            },
            new object[]
            {
                "hmac2-224 w/ nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "unexpected for macOptions"
            },
            new object[]
            {
                "hmac2-256 w/ nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildHmac2_256()
                                                                )
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "unexpected for macOptions"
            },
            new object[]
            {
                "hmac2-384 w/ nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildHmac2_384()
                                                                )
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "unexpected for macOptions"
            },
            new object[]
            {
                "hmac2-512 w/ nonce length",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithNonceLen(42)
                                                                        .BuildHmac2_512()
                                                                )
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "unexpected for macOptions"
            },
            new object[]
            {
                "invalid key length AES-CCM",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithKeyLen(new int[] { 40 })
                                                                        .BuildAesCcm()
                                                                )
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "AES Key Lengths"
            },
            new object[]
            {
                "invalid key length CMAC",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithKeyLen(new int[] { 40 })
                                                                        .BuildAesCmac()
                                                                )
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "AES Key Lengths"
            },
            new object[]
            {
                "invalid key mod non aes MAC",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithKeyLen(new int[] { 131 })
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "KeyLength Modulus"
            },
            new object[]
            {
                "keylength below minimum non AES",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .WithKeyLen(new int[] { 120 })
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "KeyLength"
            },
            new object[]
            {
                "mac below minimum AES",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(
                                                                    new MacOptionsBaseBuilder(false)
                                                                    .WithMacLen(8)
                                                                    .BuildAesCmac()
                                                                )
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "MacLength Range"
            },
            new object[]
            {
                "mac modulus AES",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(
                                                                    new MacOptionsBaseBuilder(false)
                                                                    .WithMacLen(127)
                                                                    .BuildAesCmac()
                                                                )
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "MacLength Modulus"
            },
            new object[]
            {
                "mac below minimum non-AES",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(false)
                                                                        .WithMacLen(8)
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "MacLength Range"
            },
            new object[]
            {
                "mac modulus non-AES",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder()
                                .WithKdfNoKc(
                                    new KdfNoKcBuilder()
                                        .WithParameterSets(
                                            new ParameterSetBuilder(true)
                                                .WithFc(
                                                    new ParameterSetBaseBuilder(true)
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(
                                                                    new MacOptionsBaseBuilder(false)
                                                                        .WithMacLen(127)
                                                                        .BuildHmac2_512()
                                                                )
                                                                .Build()
                                                        )
                                                        .BuildFc()
                                                )
                                                .Build()
                                        )
                                        .Build()
                                )
                                .BuildDhEphem()
                        ).Build()
                    ).Build(),
                "MacLength Modulus"
            },
        };

        [Test]
        [TestCaseSource(nameof(_testParameters))]
        public void ShouldFailValidationForExpectedReason(string label, Parameters parameters, string failureReason)
        {
            var result = _subject.Validate(parameters);

            Assume.That(!result.Success);
            Assert.IsTrue(result.ErrorMessage.ToLower().Contains(failureReason.ToLower()));
        }
    }
}
