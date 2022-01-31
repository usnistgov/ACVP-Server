using System;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ANSX942.Tests
{
    [TestFixture, FastCryptoTest]
    public class Ansi942DerTests
    {
        [TestCase]
        public void ShouldKdfCorrectly()
        {
            var sha = new NativeShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var subject = new AnsiX942Der(sha);

            var zz = new BitString("0123456789ABCDEF");
            var otherInfo = new BitString(0);

            var param = new DerAns942Parameters
            {
                Zz = zz,
                KeyLen = 256,
                Oid = AnsiX942OidHelper.GetOidFromEnum(AnsiX942Oids.TDES),
                PartyUInfo = otherInfo,
                PartyVInfo = otherInfo,
                SuppPubInfo = otherInfo,
                SuppPrivInfo = otherInfo
            };

            var result = subject.DeriveKey(param);

            Assert.IsTrue(result.Success);

            Console.WriteLine(result.DerivedKey.ToHex());
            Assert.Pass();
        }

        [TestCase]
        public void ShouldGenerateCorrectOtherInfo()
        {
            var sha = new NativeShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var subject = new AnsiX942Der(sha);

            var expectedKey = new BitString("7088B27511516F85551F20B33BB09AF453DD0ECA4542C5F48D5263D3474FC0C4");
            var param = new DerAns942Parameters
            {
                Zz = new BitString("6B"),
                KeyLen = 256,
                Oid = AnsiX942OidHelper.GetOidFromEnum(AnsiX942Oids.TDES),
                PartyUInfo = new BitString("299D468D60BC6A257E0B6523D691A3FC1602453B35F308C762FBBAC6069A88BC"),
                PartyVInfo = new BitString("80D49BFE5BE01C7D56489AB017663C22B8CBB34C3174D1D71F00CB7505AC759A"),
                SuppPubInfo = new BitString("3C21A5EA5988562C007986E0503D039E7231D9F152FE72A231A1FD98C59BCA6A"),
                SuppPrivInfo = new BitString("FD47477542989B51E4A0845DFABD6EEAA465F69B3D75349B2520051782C7F3FC")
            };

            var result = subject.DeriveKey(param);
            Assert.IsTrue(result.Success);
        }
    }
}
