using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class MCTTestGroupFactoryTests
    {
        [Test]
        public void ShouldCreateOneTestGroupForEachInstance()
        {
            var subject = new MCTTestGroupFactory();

            var results = subject.BuildMCTTestGroups(new Parameters());

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void ShouldIgnoreAnyParameters()
        {
            var subject = new MCTTestGroupFactory();
            var parameters = new ParameterBuilder()
                                    .WithDigestLen(new[] { 128 })
                                    .Build();

            var results = subject.BuildMCTTestGroups(parameters);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(160, results.First<TestGroup>().DigestLength);
        }
    }
}
