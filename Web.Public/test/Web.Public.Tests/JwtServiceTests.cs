using NUnit.Framework;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private IJwtService _jwtService;
        
        [SetUp]
        public void SetUp()
        {
            //_jwtService = new JwtService();
        }

        [Test]
        public void ShouldValidateCorrectJwtWithoutClaims()
        {
            
        }

        [Test]
        public void ShouldValidateCorrectJwtWithClaims()
        {
            
        }

        [Test]
        public void ShouldRejectInvalidJwt()
        {
            
        }

        [Test]
        public void ShouldCreateTokenWithoutClaims()
        {
            var token = _jwtService.Create();
            
            Assert.IsTrue(token.IsSuccess, token.ErrorMessage);
            Assert.IsNotNull(token.Token);
        }

        [Test]
        public void ShouldCreateTokenWithClaims()
        {
            
        }

        [Test]
        public void ShouldAddClaimsToNewTokenOnRefresh()
        {
            
        }
    }
}