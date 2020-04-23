using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.ExtensionMethods;
using NUnit.Framework;
using Web.Public.Configs;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class TotpServiceTests
    {
        private Mock<ITotpProvider> _totpProvider;
        private ITotpService _totpService;
        private TotpConfig _totpConfig;
        private OptionsWrapper<TotpConfig> _options;
        private byte[] _dummyCert;

        [SetUp]
        public void SetUp()
        {
            _totpProvider = new Mock<ITotpProvider>();
            _totpProvider
                .Setup(s => s.GetSeedFromUserCertificate(It.IsAny<byte[]>()))
                .Returns(new byte[] {1});

            _totpProvider
                .Setup(s => s.GetUsedWindowFromUserCertificate(It.IsAny<byte[]>()))
                .Returns(1);
            
            _totpConfig = new TotpConfig
            {
                Digits = 8,
                EnforceUniqueness = true,
                Hmac = "SHA256",
                Step = 30
            };
            
            _options = new OptionsWrapper<TotpConfig>(_totpConfig);

            _dummyCert = new byte[] {0, 1, 2, 3};
            
            _totpService = new TotpService(_totpProvider.Object, _options);
        }

        [Test]
        public void ShouldGenerateValidTotpPasswords()
        {
            var totp = _totpService.GenerateTotp(_dummyCert);
            
            Assert.AreEqual(_options.Value.Digits, totp.Length, "totp password length");
            Assert.IsTrue(totp.All(char.IsDigit), "totp all digits");
        }

        [Test]
        public void ShouldValidateNewlyGeneratedTotpPassword()
        {
            // RFC specification allows verifier to check back 1 time window for verification based on network travel time
            var totp = _totpService.GenerateTotp(_dummyCert);
            var result = _totpService.ValidateTotp(_dummyCert, totp);
            
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void ShouldRejectRandomTotpPassword()
        {
            var totp = _totpService.GenerateTotp(_dummyCert);
            var randomTotp = new string(totp.ToCharArray().ToList().Shuffle().ToArray());
            
            Assert.AreEqual(_options.Value.Digits, randomTotp.Length, "random totp length");

            var result = _totpService.ValidateTotp(_dummyCert, randomTotp);
            
            Assert.IsFalse(result.IsSuccess, "success check");
        }

        [Test]
        public void ShouldGenerateSameTotpPasswordForSameTimeWindow()
        {
            // Need to generate 3 values in the small chance that the time step occurs between the first and second value
            var totp1 = _totpService.GenerateTotp(_dummyCert);
            var totp2 = _totpService.GenerateTotp(_dummyCert);
            var totp3 = _totpService.GenerateTotp(_dummyCert);
            
            Assert.IsTrue(totp1.Equals(totp2) || totp2.Equals(totp3));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldRelyOnConfigForUniquenessRequirement(bool uniquenessRequired)
        {
            var expectedWindow = CalculateTimeStepFromTimestamp(DateTime.Now);
            
            _totpProvider
                .Setup(s => s.GetUsedWindowFromUserCertificate(It.IsAny<byte[]>()))
                .Returns(expectedWindow);

            _totpConfig.EnforceUniqueness = uniquenessRequired;
            _options = new OptionsWrapper<TotpConfig>(_totpConfig);
            
            _totpService = new TotpService(_totpProvider.Object, _options);
            var totp = _totpService.GenerateTotp(_dummyCert);
            var result = _totpService.ValidateTotp(_dummyCert, totp);

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
            _totpService = new TotpService(_totpProvider.Object, _options);

            Assert.Throws<Exception>(() => _totpService.GenerateTotp(_dummyCert), "generate totp");
            Assert.Throws<Exception>(() => _totpService.ValidateTotp(_dummyCert, "dummy password"), "validate totp");
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