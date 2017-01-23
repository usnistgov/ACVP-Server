using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        // Only one possible generator
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var result = subject.GetCaseGenerator();
            Assert.IsNotNull(result);
        }
    }
}
