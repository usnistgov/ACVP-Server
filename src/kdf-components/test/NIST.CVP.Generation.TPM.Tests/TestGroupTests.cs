using NIST.CVP.Generation.TPMv1._2;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TPM.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.IsFalse(result);
        }
    }
}
