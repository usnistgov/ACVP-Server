using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.SSC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetXecdhSscTestAsync(It.IsAny<XecdhSscParameters>()))
                .Returns(() => Task.FromResult(new XecdhSscResult
                {
                    PrivateKeyIut = new BitString("AA"),
                    PrivateKeyServer = new BitString("BB"),
                    PublicKeyIut = new BitString("CC"),
                    PublicKeyServer = new BitString("DD"),
                    Z = new BitString("00")
                }));

            _subject = new TestCaseGenerator(_oracle.Object);
        }

        private TestCaseGenerator _subject;

        private Mock<IOracle> _oracle;

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGenerateSuccessfully(bool isSample)
        {
            var result = await _subject.GenerateAsync(GetTestGroup(), isSample);

            Assert.That(result.Success, Is.True);
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
