using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class KdfKmacTests
    {
        KdfOneStepFactory _kdfOneStepFactory = new KdfOneStepFactory(new NativeShaFactory(), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));

        private static object[] _kdfTestCases = new object[]
        {
            new object[]
            {
				// label
				"https://github.com/usnistgov/ACVP-Server/issues/43#issue-752020792",
				// fixed info pattern
				"algorithmId||uPartyInfo||vPartyInfo",
				// KDF type
				Kda.OneStep,
				// Aux Function
				KdaOneStepAuxFunction.KMAC_128,
				// l
				1024,
				// salt
				new BitString("00000000000000000000000000000000"),
				// z
				new BitString("39CB5C7486EEB4CFA379EC16CC23C8D9E0C3FCC941B92B6C8F1658F5929BB48D"),
				// algorithmId
				new BitString("C90F0C695C8372FCB94F0BAF9A86AC67"),
				// partyU partyId
				new BitString("00F00E52832EF7960EBD7A46E2E7F574"),
				// partyU ephem data
				new BitString("C4C150F5BD1F35140B5F9AE014D3CD7F600F1B8F6940C3F83AA0B343E8A082A2"),
				// partyV partyId
				new BitString("DB06A47DB74EA7474231502AD9D1D7EE"), 
				// partyV ephem data
				null,
				// issue's fixed info
				new BitString("c90f0c695c8372fcb94f0baf9a86ac6700f00e52832ef7960ebd7a46e2e7f574c4c150f5bd1f35140b5f9ae014d3cd7f600f1b8f6940c3f83aa0b343e8a082a2db06a47db74ea7474231502ad9d1d7ee"),
				// issue's dkm
				new BitString("80a3dd756f342e8f29dfbb4c9d05f19d1f8ac29246bda54627fc7b064359ff62c3566f148fead0f2053d9637e6ce334b3067e66068da811a2f6197c615c9c13f2c3bb7da0392481fc06c96d81e9a6e38d9d1228e3fd9e88e4891f4bbb1dc59adb3fb2b2af956219b3201c8f7b658089529e36929c568c014d1cc9719991e9861"),
				// expected dkm
				new BitString("D6572FACBFC553F16FD7B7D58F95E3E4A24D6C336BF7F9E7171914B7194AAC45C044FA5F754D1F4850AB1BBDA04AAACEED115647C5F9FE6A40A10FD4654E9E6ACBE9C6DB3548DAA376E9E5C1AE30B881A1BE42933D7D3FD6664B9E2F5EEA234870952D2E0608D89269791D6B4E925D5B6B5B60894B1B968CA173080984B225CD")
            }
        };

        [Test]
        [TestCaseSource(nameof(_kdfTestCases))]
        public void ShouldKmac(
            string label,
            string fixedInfoPattern,
            Kda kdfType,
            KdaOneStepAuxFunction kdfAuxFunction,
            int l,
            BitString salt,
            BitString z,
            BitString algorithmId,
            BitString uPartyId,
            BitString uEphemData,
            BitString vPartyid,
            BitString vEphemData,
            BitString issueFixedData,
            BitString issueDkm,
            BitString expectedDkm)
        {
            var param = new KdfParameterOneStep()
            {
                L = l,
                Salt = salt,
                Z = z,
                AlgorithmId = algorithmId,
                AuxFunction = kdfAuxFunction,
                FixedInfoPattern = fixedInfoPattern,
                FixedInputEncoding = FixedInfoEncoding.Concatenation
            };

            var fixedInfoFactory = new FixedInfoFactory(new FixedInfoStrategyFactory());
            var fixedInfoParameter = new FixedInfoParameter()
            {
                AlgorithmId = algorithmId,
                FixedInfoPattern = fixedInfoPattern,
                Encoding = FixedInfoEncoding.Concatenation,
                L = l,
                Salt = salt
            };

            fixedInfoParameter.SetFixedInfo(
                new PartyFixedInfo(uPartyId, uEphemData),
                new PartyFixedInfo(vPartyid, vEphemData));

            var fixedInfo = fixedInfoFactory
                .Get()
                .Get(fixedInfoParameter);

            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction, true);

            var result = kdf.DeriveKey(param.Z, param.L, fixedInfo, param.Salt);

            Assert.AreEqual(issueFixedData.ToHex(), fixedInfo.ToHex(), nameof(issueFixedData));
            Assert.AreEqual(issueDkm.ToHex(), result.DerivedKey.ToHex(), nameof(issueDkm));
            //Assert.AreEqual(expectedDkm.ToHex(), result.DerivedKey.ToHex(), nameof(expectedDkm));
        }
    }
}
