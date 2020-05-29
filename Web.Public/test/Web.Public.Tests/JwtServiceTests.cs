using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Web.Public.Configs;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private class JwtServiceVirtualCreationTime : JwtService
        {
            protected override DateTime JwtCreateDateTime { get; }

            public JwtServiceVirtualCreationTime(ILogger<JwtService> logger, IOptions<JwtConfig> jwtOptions, ISecretKvpProvider secretKvpProvider, DateTime jwtCreationDateTime) : base(logger, jwtOptions, secretKvpProvider)
            {
                JwtCreateDateTime = jwtCreationDateTime;
            }
        }
        
        private IJwtService _jwtService;
        private Mock<ILogger<JwtService>> _logger;
        private Mock<IOptions<JwtConfig>> _jwtConfig;
        private Mock<ISecretKvpProvider> _secretKeyProvider;
        private const string MyCertSubject = "This is a legit cert subject";
        
        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<JwtService>>();
            _jwtConfig = new Mock<IOptions<JwtConfig>>();
            _jwtConfig.Setup(s => s.Value).Returns(new JwtConfig()
            {
                Issuer = "My test issuer",
                FriendlySignatureScheme = "HmacSha256",
                ValidWindowHours = 0,
                ValidWindowMinutes = 30,
                ValidWindowSeconds = 0
            });
            _secretKeyProvider = new Mock<ISecretKvpProvider>();
            _secretKeyProvider
                .Setup(s => s.GetValueFromKey(It.IsAny<string>()))
                .Returns("my testing secret key, the good one for testing, we may overwrite the key at some point to make sure the jwt does not validate");
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);
        }

        [Test]
        public void ShouldCreateAndValidateJwt()
        {
            var jwt = _jwtService.Create(MyCertSubject, null);
            Assert.True(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, true));
        }

        [Test]
        public void ShouldAddClaimToJwt()
        {
            var kvp = new KeyValuePair<string, string>("myClaim", "claimAdded");
            var claimsToAdd = new Dictionary<string, string>();
            claimsToAdd.Add(kvp.Key, kvp.Value);
            
            var jwt = _jwtService.Create(MyCertSubject, claimsToAdd);
            var claimsFromJwt = _jwtService.GetClaimsFromJwt(jwt.Token);
            
            Assert.IsTrue(claimsFromJwt.ContainsKey(kvp.Key), "key exists");
            Assert.AreEqual(kvp.Value, claimsFromJwt[kvp.Key]);
        }

        [Test]
        public void ShouldNotValidateWithDifferingSubjectClaims()
        {
            var jwt = _jwtService.Create(MyCertSubject, null);
            Assert.False(_jwtService.IsTokenValid(MyCertSubject + "doot", jwt.Token, true));
        }

        [Test]
        public void ShouldNotValidateWithDifferentSecretKey()
        {
            var jwt = _jwtService.Create(MyCertSubject, null);
            
            _jwtConfig.Setup(s => s.Value).Returns(new JwtConfig()
            {
                Issuer = "My test issuer",
                FriendlySignatureScheme = "HmacSha256",
                ValidWindowHours = 0,
                ValidWindowMinutes = 30,
                ValidWindowSeconds = 0
            });
            _secretKeyProvider
                .Setup(s => s.GetValueFromKey(It.IsAny<string>()))
                .Returns("baby shark doot doot doot doo doo doo, baby shark doot doot doot doo doo doo, baby shark doot doot doot doo doo doo, baby shark!");
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);
            
            Assert.IsFalse(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, true));
        }

        [Test]
        public void ShouldValidateIfJwtExpiredAndIgnoringExpiration()
        {
            _jwtService = new JwtServiceVirtualCreationTime(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            
            var jwt = _jwtService.Create(MyCertSubject, null);
            
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);

            Assert.True(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, false));
        }
        
        [Test]
        public void ShouldNotValidateIfJwtExpiredAndNotIgnoringExpiration()
        {
            _jwtService = new JwtServiceVirtualCreationTime(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            
            var jwt = _jwtService.Create(MyCertSubject, null);
            
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);

            Assert.False(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, true));
        }
        
        [Test]
        public void ShouldNotValidateIfJwtExpired()
        {
            _jwtService = new JwtServiceVirtualCreationTime(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            
            var jwt = _jwtService.Create(MyCertSubject, null);
            
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);

            Assert.False(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, true));
        }

        [Test]
        public void TokenShouldRefreshAndValidate()
        {
            var jwt = _jwtService.Create(MyCertSubject, null);
            var refreshJwt = _jwtService.Refresh(MyCertSubject, jwt.Token);
            
            Assert.True(_jwtService.IsTokenValid(MyCertSubject, refreshJwt.Token, true));
        }

        [Test]
        public void TokenShouldTransferClaimsOnRefresh()
        {
            var kvp = new KeyValuePair<string, string>("myClaim", "claimAdded");
            var claimsToAdd = new Dictionary<string, string>();
            claimsToAdd.Add(kvp.Key, kvp.Value);
            
            var jwt = _jwtService.Create(MyCertSubject, claimsToAdd);
            var refreshJwt = _jwtService.Refresh(MyCertSubject, jwt.Token);

            var claimsFromRefreshedJwt = _jwtService.GetClaimsFromJwt(refreshJwt.Token);
            
            Assert.IsTrue(claimsFromRefreshedJwt.ContainsKey(kvp.Key), "key exists");
            Assert.AreEqual(kvp.Value, claimsFromRefreshedJwt[kvp.Key]);
        }

        [Test]
        public void TokenShouldRefreshEvenWhenExpired()
        {
            _jwtService = new JwtServiceVirtualCreationTime(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
            
            var jwt = _jwtService.Create(MyCertSubject, null);
            
            _jwtService = new JwtService(_logger.Object, _jwtConfig.Object, _secretKeyProvider.Object);

            var refreshToken = _jwtService.Refresh(MyCertSubject, jwt.Token);
            
            // The "old" token should not be valid due to being expired
            Assert.False(_jwtService.IsTokenValid(MyCertSubject, jwt.Token, true));
            // The "refresh" token should be valid even though it was created off of an expired token.
            Assert.True(_jwtService.IsTokenValid(MyCertSubject, refreshToken.Token, true));
        }
    }
}