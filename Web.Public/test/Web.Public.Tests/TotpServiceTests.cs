using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Math;
using NUnit.Framework;
using OtpNet;
using Web.Public.Configs;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class TotpServiceTests
    {
        private Mock<ILogger<TotpService>> _logger = new Mock<ILogger<TotpService>>();
        private Mock<ITotpProvider> _totpProvider;
        private ITotpService _totpService;
        private TotpConfig _totpConfig;
        private OptionsWrapper<TotpConfig> _options;
        private string _dummyCertSubject;

        [SetUp]
        public void SetUp()
        {
            _totpProvider = new Mock<ITotpProvider>();
            _totpProvider
                .Setup(s => s.GetSeedFromUserCertificateSubject(It.IsAny<string>()))
                .Returns(new byte[] {1});

            _totpProvider
                .Setup(s => s.GetUsedWindowFromUserCertificateSubject(It.IsAny<string>()))
                .Returns(1);
            
            _totpConfig = new TotpConfig
            {
                Digits = 8,
                EnforceUniqueness = true,
                Hmac = "SHA256",
                Step = 30
            };
            
            _options = new OptionsWrapper<TotpConfig>(_totpConfig);

            _dummyCertSubject = string.Empty;
            
            _totpService = new TotpService(_logger.Object, _totpProvider.Object, _options);
        }

        [Test]
        public void ShouldGenerateValidTotpPasswords()
        {
            var totp = _totpService.GenerateTotp(_dummyCertSubject);
            
            Assert.AreEqual(_options.Value.Digits, totp.Length, "totp password length");
            Assert.IsTrue(totp.All(char.IsDigit), "totp all digits");
        }

        [Test]
        public void ShouldValidateNewlyGeneratedTotpPassword()
        {
            // RFC specification allows verifier to check back 1 time window for verification based on network travel time
            var totp = _totpService.GenerateTotp(_dummyCertSubject);
            var result = _totpService.ValidateTotp(_dummyCertSubject, totp);
            
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void ShouldRejectRandomTotpPassword()
        {
            var totp = _totpService.GenerateTotp(_dummyCertSubject);
            var randomTotp = new string(totp.ToCharArray().ToList().Shuffle().ToArray());
            
            Assert.AreEqual(_options.Value.Digits, randomTotp.Length, "random totp length");

            var result = _totpService.ValidateTotp(_dummyCertSubject, randomTotp);
            
            Assert.IsFalse(result.IsSuccess, "success check");
        }

        [Test]
        public void ShouldGenerateSameTotpPasswordForSameTimeWindow()
        {
            // Need to generate 3 values in the small chance that the time step occurs between the first and second value
            var totp1 = _totpService.GenerateTotp(_dummyCertSubject);
            var totp2 = _totpService.GenerateTotp(_dummyCertSubject);
            var totp3 = _totpService.GenerateTotp(_dummyCertSubject);
            
            Assert.IsTrue(totp1.Equals(totp2) || totp2.Equals(totp3));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldRelyOnConfigForUniquenessRequirement(bool uniquenessRequired)
        {
            var expectedWindow = CalculateTimeStepFromTimestamp(DateTime.Now);
            
            _totpProvider
                .Setup(s => s.GetUsedWindowFromUserCertificateSubject(It.IsAny<string>()))
                .Returns(expectedWindow);

            _totpConfig.EnforceUniqueness = uniquenessRequired;
            _options = new OptionsWrapper<TotpConfig>(_totpConfig);
            
            _totpService = new TotpService(_logger.Object, _totpProvider.Object, _options);
            var totp = _totpService.GenerateTotp(_dummyCertSubject);
            var result = _totpService.ValidateTotp(_dummyCertSubject, totp);

            Assert.AreEqual(uniquenessRequired, !result.IsSuccess);
        }

        [Test]
        [TestCase("")]
        [TestCase("sha2-256")]
        [TestCase("sha")]
        public void ShouldThrowWhenConfigHmacIsImproper(string hmac)
        {
            _totpConfig.Hmac = hmac;
            _options = new OptionsWrapper<TotpConfig>(_totpConfig);
            _totpService = new TotpService(_logger.Object, _totpProvider.Object, _options);

            Assert.Throws<Exception>(() => _totpService.GenerateTotp(_dummyCertSubject), "generate totp");
            Assert.Throws<Exception>(() => _totpService.ValidateTotp(_dummyCertSubject, "dummy password"), "validate totp");
        }

        [Test]
        public void TestGoogleAuthenticator()
        {
	        var totp = new Totp(Convert.FromBase64String("YZF2OplkPQ5MzPcyOuzBd7MeyhO3gpiA"), 30, OtpHashMode.Sha1, 6);
	        Assert.AreEqual("423442", totp.ComputeTotp(new DateTime(1522966271)));
        }

        [Test]
        public void TestKatsFromHotpRfc()
        {
            // Test vectors provided from RFC 4226 
            Hotp hotp = new Hotp(new BitString("3132333435363738393031323334353637383930").ToBytes(), OtpHashMode.Sha1, 6);
            string[] tests = {
                "755224",
                "287082",
                "359152",
                "969429",
                "338314",
                "254676",
                "287922",
                "162583",
                "399871",
                "520489",
            };
            Assert.Multiple(() =>
            {
                for (int i = 0; i < tests.Length; i++) {
                    Assert.AreEqual(tests[i], hotp.ComputeHOTP(i));
                }                
            });
        }
        
        [Test]
        public void TestKatsFromTotpRfc()
        {
            Totp totp = new Totp(new BitString("3132333435363738393031323334353637383930").ToBytes(), 30, OtpHashMode.Sha1, 8);
            Totp totp32 = new Totp(new BitString("3132333435363738393031323334353637383930313233343536373839303132").ToBytes(), 30, OtpHashMode.Sha256, 8);
            Totp totp64 = new Totp(new BitString("31323334353637383930313233343536373839303132333435363738393031323334353637383930313233343536373839303132333435363738393031323334").ToBytes(), 30, OtpHashMode.Sha512, 8);

            long[] testTime = {59L, 1111111109L, 1111111111L, 1234567890L, 2000000000L, 20000000000L};
            string[] test = {
                "94287082",
                "07081804",
                "14050471",
                "89005924",
                "69279037",
                "65353130"
            };
            string[] test32 = {
                "46119246",
                "68084774",
                "67062674",
                "91819424",
                "90698825",
                "77737706"
            };
            string[] test64 = {
                "90693936",
                "25091201",
                "99943326",
                "93441116",
                "38618901",
                "47863826"
            };
            Assert.Multiple(() =>
            {
                for (int i = 0; i < testTime.Length; i++) {
                    Assert.AreEqual(test[i], totp.ComputeTotp(new DateTime(testTime[i])), $"failed SHA1 iteration {i}");
                    Assert.AreEqual(test32[i], totp32.ComputeTotp(new DateTime(testTime[i])), $"failed SHA256 iteration {i}");
                    Assert.AreEqual(test64[i], totp64.ComputeTotp(new DateTime(testTime[i])), $"failed SHA512 iteration {i}");
                }
            });
        }
        
        // Grabbed from OtpNet (TOTP library) to compute the expected timeWindow based on current time
        private long CalculateTimeStepFromTimestamp(DateTime timestamp)
        {
            const long unixEpochTicks = 621355968000000000L;
            const long ticksToSeconds = 10000000L;
            
            var unixTimestamp = (timestamp.Ticks - unixEpochTicks) / ticksToSeconds;
            var window = unixTimestamp / _options.Value.Step;
            return window;
        }
    }
}