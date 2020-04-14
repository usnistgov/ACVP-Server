using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Web.Public.Configs;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private class TestJwtService : JwtService
        {
            public TestJwtService(ILogger<JwtService> logger, IOptions<JwtConfig> jwtOptions) : base(logger, jwtOptions)
            {
            }

            public Dictionary<string, string> GetClaims(JwtSecurityToken jwt)
            {
                return GetClaimsFromJwt(jwt);
            }
        }

        private TestJwtService _jwtService;
        private Mock<ILogger<JwtService>> _logger;
        private Mock<IOptions<JwtConfig>> _jwtConfig;
        private const string MyCertSubject = "This is a legit cert subject";
        
        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<JwtService>>();
            _jwtConfig = new Mock<IOptions<JwtConfig>>();
            _jwtConfig.Setup(s => s.Value).Returns(new JwtConfig()
            {
                Issuer = "My test issuer",
                SecretKey = "my testing secret key, the good one for testing, we may overwrite the key at some point to make sure the jwt does not validate",
                FriendlySignatureScheme = "HmacSha256",
                ValidWindowHours = 0,
                ValidWindowMinutes = 30,
                ValidWindowSeconds = 0
            });
            _jwtService = new TestJwtService(_logger.Object, _jwtConfig.Object);
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var readJwt = tokenHandler.ReadJwtToken(jwt.Token);
            var claimsFromJwt = _jwtService.GetClaims(readJwt);
            
            Assert.IsTrue(claimsFromJwt.ContainsKey(kvp.Key), "key exists");
            Assert.AreEqual(kvp.Value, claimsFromJwt[kvp.Key]);
        }

        public void ShouldNotValidateWithDifferingSubjectClaims()
        {
            Assert.Fail();
        }
    }
}