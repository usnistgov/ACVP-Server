using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture, LongCryptoTest]
    public class RandomProbablePrimeGeneratorTests
    {
        [Test]
        [TestCase(0, "010001")]
        [TestCase(2048, "03")]
        public void ShouldFailWithBadParametersFips186_4(int nlen, string e)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger()
            };

            var subject = new RandomProbablePrimeGenerator(new EntropyProvider(new Random800_90()), PrimeTestModes.TwoPow100ErrorBound);

            Assert.Throws<RsaPrimeGenException>(() => subject.GeneratePrimesFips186_4(param));
        }

        [Test]
        [TestCase(1024, "03")]
        [TestCase(1536, "11")]
        [TestCase(2048, "010001")]
        [TestCase(3072, "03")]
        [TestCase(4096, "11")]
        public void ShouldPassWithGoodParametersFips186_2(int nlen, string e)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger()
            };

            var subject = new RandomProbablePrimeGenerator(new EntropyProvider(new Random800_90()), PrimeTestModes.TwoPow100ErrorBound);

            var result = subject.GeneratePrimesFips186_2(param);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, "df28ab")]
        [TestCase(2048, "e66d81")]
        [TestCase(3072, "df28ab")]
        [TestCase(3072, "e66d81")]
        [TestCase(4096, "010001")]
        public void ShouldPassWithGoodParametersFips186_4(int nlen, string e)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger()
            };

            var subject = new RandomProbablePrimeGenerator(new EntropyProvider(new Random800_90()), PrimeTestModes.TwoPow100ErrorBound);

            var result = subject.GeneratePrimesFips186_4(param);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, "df28ab", 1, 3)]
        [TestCase(2048, "010001", 7, 7)]
        [TestCase(3072, "010001", 3, 5)]
        [TestCase(4096, "e66d81", 5, 0)]
        public void ShouldPassWithGoodParametersFips186_5(int nlen, string e, int a, int b)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger(),
                A = a,
                B = b
            };

            var subject = new RandomProbablePrimeGenerator(new EntropyProvider(new Random800_90()), PrimeTestModes.TwoPow100ErrorBound);

            var result = subject.GeneratePrimesFips186_5(param);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
            if (a != 0) Assert.That((int)(result.Primes.P % 8), Is.EqualTo(a));
            if (b != 0) Assert.That((int)(result.Primes.Q % 8), Is.EqualTo(b));
        }

        // These are KATs
        [Test]
        [TestCase(2048, "df28ab",
            "e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51",
            "ed1571a9e0cd4a42541284a9f98b54a6af67d399d55ef888b9fe9ef76a61e892c0bfbb87544e7b24a60535a65de422830252b45d2033819ca32b1a9c4413fa721f4a24ebb5510ddc9fd6f4c09dfc29cb9594650620ff551a62d53edc2f8ebf10beb86f483d463774e5801f3bb01c4d452acb86ecfade1c7df601cab68b065275",
            true, "pass")]
        [TestCase(2048, "85a4cf",
            "e534f4a4eb86ff9ace08a0b446faf3e20c22a0166057507e4f5f07332d5c0878a50798857d5e9946e3f8ef8a1021481bb0c94631f9ad8427df620ec9ca585cab3082222279f41bc40e2ccdc160dbc410c52662699ae16b27b2c9d2bf14e99083920a448ba4e5d3d11e1ab7777613959c07fb213be26f2cb7ea8a759af082f6c5",
            "00",
            false, "p not prime")]
        [TestCase(2048, "cf88fb",
            "0056d803b5c05e177868666a0b2f050628d558ec25c6dcbd9af7099da0e7982748ec336cb0af86a65baf133058c1167d3158f445857bade39b1e8c1f1e8f1925482c6b92a1558c83bfea20acce6f95a2011a29e7f5e20759361627e171513580f07041c4b2525c4bc03a1f7e97cffed49bf4aa53a8ea49a78aaf1c7979beed95",
            "00",
            false, "p < sqrt(2) * 2 ^ (n / 2) - 1, so get a new p")]
        [TestCase(2048, "85a4cf",
            "d1e44501f8a1793adc43c9083e0a893718b2c7a6cbabfec8785569f4db751e8126540645458a5033b8496d8ac7d5634afcbfffbe52d8aac30b50b91010a5beced69b792cac57aca3dea346beed96018002b8b36b28665113ddf0c138639dee7b3d9dc7d4b1e404d3fbc214e0c4d0a91ac8f15c2bcae338f5b349e6cc43babfe9",
            "d75a4a175c161cf03da85eb1679f80115e48097090032abd73170db1522754682299e450f5eda45496b33ec92274d5e311bb28ebb6102ebe93e497882ed80a9f254886fe188380f7ba02110c9781fe6b1ab22ab145f6ca1a1c0526427b38e13e1792df72372a411e78c3e57eabc19b91c9e7fa492afcda00986df8c61e2cab3f",
            false, "q not prime")]
        [TestCase(2048, "a43b41",
            "d2bccf46a2c6724fc76abdb99b5bc62826008c33bbc4581686e683a0c5cdf92027fa7cc0df7c97ae098e9c02bd6dca18f5a537deb694aefb5acd6a1078b1a03ee0a4855d4645feaed3a16087e18f02e5395b888a07145965ae1b719777a9926e5bb0a1be1eeaa0de375572d2d0138813835fa7cbcb0a967cafc74a1c4e608c7d",
            "ef152aa35b36fe8248f753b68ec65e73cd77208c54562101f1063a983ebbfa6dd123f6656f03abe250fe3a6ed00e641594526f2d95917e93d61f63edf029e7602dbd95d0ed7c8f6c32703a855478eaedfbe6273455acd04b914c92b811381a09c5426d566ef53ad9bca9583bf9def3e90f3db386975aac84cdeed42d22aa13d8",
            false, "q not prime")]
        [TestCase(2048, "e66d81",
            "fb61c111b038153b645cdd3103fc5eb3e9ab09b64d11de97a08662c569fb22456203fa5fc6b7e41a8e83fe995eeaea9cca670575a662447d39012aa093a051e781df6018c0ea8ab76d49353363074e92f070dfe3c3c8964acad4532da8bea7b0944ffd229f06da23abe7b050418abe4b44513777b988ab30ee696ef053e23ca5",
            "ebb6e652fcc1ba4dd72f5e0c5409c6bcde63f5781cd69a785045db14312d96d13809f96a20ce9c5417c9d01ec2b947c1c180ae208ddc88e8c140da1a241d27ba8c9ce33ff8f97334566bf99a7942b29c5663f8de4cdbcfc43659d5a1b1111f8c87b0d7346da6f7f16dafcdb1f014495c9f4f5635d4fa5ec48cdd323aa9dba968",
            false, "q not prime")]
        public void ShouldPassKATsFips186_4(int nlen, string e, string pRand, string qRand, bool expectedResult, string expectedMessage)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger()
            };

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(new BitString(pRand));
            entropyProvider.AddEntropy(new BitString(qRand));

            var subject = new RandomProbablePrimeGenerator(entropyProvider, PrimeTestModes.TwoPow100ErrorBound);

            var result = subject.GeneratePrimesKat(param);

            Assert.That(result.Success, Is.EqualTo(expectedResult), expectedMessage);
            if (!result.Success)
            {
                Console.Write($"Expected reason: {expectedMessage}\nActual reason: {result.ErrorMessage}");
            }
        }

        /// <summary>
        /// https://github.com/usnistgov/ACVP/issues/1153
        /// </summary>
        [Test]
        public void ShouldBeWithinBounds()
        {
            var mistakenLowerBounds = PrimeGeneratorHelper.Root2Mult2Pow2048Minus1;

            var actualLowerBounds = PrimeGeneratorHelper.Root2Mult2Pow3072Minus1;

            var p = new BitString(
                    "43437730C34C04A701B1C1BDC848140EBBB6893626568567DA9D439F8B94D352A26DBE6377A9F3DACFD8558B6E85EF9AAF051A96B9E22802A6EEDAB727D380563C3042B7E3C413D54C42194E582C56B4546A720BEA0F83E525A2278A7F401CE827ADB8C53A4304221004A53CC84DA4CE7959F744BCA703F5F57DFBE7E624DAF27EE19CFCFBD00E4BAE2EB2ED04786320890EAF5A339BC9C3AAB495498FB251C2F2AAB42B6D2EDD28B32ADF856AADA98AC68B9A38E7CD9D330C0A2AD9F8C2768EB41C9BAE0329AB24469627D0CED6CDD48D8E0B064106DF8E58B855195224BEDBFBAE5B37823619D191DB3EC65BF6EB61237DFEB79D723C10541DE41CFB3D00855D83C7B938F1528B45A9797D1634AC3DD4505653A91B5EC3689428D4C25F7A0A5855FB0733C0B27AD0A7D3FC35A4A47DF2A67B764859DA588BFB7AEA6B233319A5AE21BBB29EE450C79EAC8474E9BEE27E94F8F57820B97A10AEA618117A197D2C48E6F4862312EA9CF49701D3EB0731138B6585A15F618BA2A7DFC58981455F")
                .ToPositiveBigInteger();

            Assert.That(p > mistakenLowerBounds, Is.True, nameof(mistakenLowerBounds));
            Assert.That(p < actualLowerBounds, Is.True, nameof(actualLowerBounds));
        }
    }
}
