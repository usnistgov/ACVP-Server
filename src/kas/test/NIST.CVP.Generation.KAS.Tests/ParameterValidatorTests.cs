using NIST.CVP.Generation.KAS.Tests.Builders;
using NIST.CVP.Generation.KAS.v1_0;
using NIST.CVP.Generation.KAS.v1_0.FFC;
using NIST.CVP.Math.Domain;
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
            Parameters p = _builder
                .WithSchemes(
                    new SchemesBuilder()
                        .WithDhEphem(SchemeBuilder.GetBaseDhEphemBuilder().Build<FfcDhEphem>())
                        .WithMqv1(SchemeBuilder.GetBaseMqv1Builder().Build<FfcMqv1>())
                        .BuildSchemes())
                .BuildParameters();

            var result = _subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        private static object[] _testParameters = new object[]
        {
            new object[]
            {
                "null algorithm",
                new ParameterBuilder().WithAlgorithm(null).BuildParameters(),
                "algorithm"
            },
            new object[]
            {
                "invalid algorithm",
                new ParameterBuilder().WithAlgorithm("invalid").BuildParameters(),
                "algorithm"
            },
            new object[]
            {
                "null function",
                new ParameterBuilder().WithFunctions(null).BuildParameters(),
                "function"
            },
            new object[]
            {
                "invalid function",
                new ParameterBuilder().WithFunctions(new string[] { "invalid" }).BuildParameters(),
                "function"
            },
            new object[]
            {
                "null schemes",
                new ParameterBuilder().WithSchemes(null).BuildParameters(),
                "scheme"
            },
            new object[]
            {
                // TODO update with rest of schemes as they're added to ensure there are zero valid schemes provided
                "No valid schemes",
                new ParameterBuilder()
                .WithSchemes(
                    new SchemesBuilder().WithDhEphem(null).BuildSchemes()
                ).BuildParameters(),
                "scheme"
            },
            new object[]
            {
                "null role dhEphem",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder().WithRole(null).Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
                "role"
            },
            new object[]
            {
                "invalid role dhEphem",
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder().WithDhEphem(
                            new SchemeBuilder().WithRole(new string[] { "invalid" }).Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                    new NoKdfNoKcBuilder().WithParameterSets(null).BuildNoKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                    new KdfNoKcBuilder().WithParameterSets(null).BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                    new KdfNoKcBuilder().WithKdfOptions(null).BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                            new KdfOptionsBuilder().WithAsn1("invalid||uPartyInfo").BuildKdfOptions()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                            new KdfOptionsBuilder().WithAsn1("literal[nonHex]||uPartyInfo").BuildKdfOptions()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildNoKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                .BuildParameterSetBaseFc()
                                        )
                                        .BuildParameterSets()
                                    )
                                    .BuildNoKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                            new ParameterSetBuilder(true).BuildParameterSets()
                                        )
                                        .BuildNoKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                            new ParameterSetBuilder(false).BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                        .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(40)))
                                                                        .BuildAesCcm()
                                                                )
                                                                .WithCmac(null)
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                        .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(40)))
                                                                        .BuildAesCmac()
                                                                )
                                                                .WithHmac2_224(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                        .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(131)))
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                        .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(120)))
                                                                        .BuildHmac2_224()
                                                                )
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc()
                                                )
                                                .BuildParameterSets()
                                        )
                                        .BuildKdfNoKc()
                                )
                                .Build<FfcDhEphem>()
                        ).BuildSchemes()
                    ).BuildParameters(),
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

        private static object[] _schemeBuilders_ShouldSucceedWhenOnlyOneSchemePresent = new object[]
        {
            new object[]
            {
                new ParameterBuilder()
                .WithSchemes(
                    new SchemesBuilder()
                        .WithDhEphem(
                            SchemeBuilder
                                .GetBaseDhEphemBuilder()
                                .Build<FfcDhEphem>()
                        )
                        .BuildSchemes()
                )
                .BuildParameters()
            },
            new object[]
            {
                new ParameterBuilder()
                .WithSchemes(
                    new SchemesBuilder()
                        .WithMqv1(
                            SchemeBuilder
                                .GetBaseMqv1Builder()
                                .Build<FfcMqv1>()
                        )
                        .BuildSchemes()
                )
                .BuildParameters()
            },
            // TODO more schemes
        };

        [Test]
        [TestCaseSource(nameof(_schemeBuilders_ShouldSucceedWhenOnlyOneSchemePresent))]
        public void ShouldSucceedWhenOnlyOneSchemePresent(Parameters parameters)
        {
            var result = _subject.Validate(parameters);

            Assert.IsTrue(result.Success);
        }
    }
}
