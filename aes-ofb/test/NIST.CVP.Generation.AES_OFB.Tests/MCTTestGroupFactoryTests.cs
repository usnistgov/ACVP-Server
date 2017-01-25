using System.Linq;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    public class MCTTestGroupFactoryTests
    {
        [TestFixture]
        public class KATTestGroupFactoryTests
        {
            private static object[] parameters = new object[]
            {
            new object[]
            {
                0, // 1 * 0 * 1
                new ParameterBuilder()
                    .WithKeyLen(new int[] { }) // 0
                    .WithMode(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 * 1
                new ParameterBuilder()
                    .WithKeyLen(new[] {128}) // 1
                    .WithMode(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                4, // 1 * 2 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192}) // 2
                    .WithMode(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            },
            new object[]
            {
                6, // 1 * 3 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192, 256}) // 3
                    .WithMode(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            }
            };
            [Test]
            [TestCaseSource(nameof(parameters))]
            public void ShouldCreate1TestGroupForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
            {
                MCTTestGroupFactory subject = new MCTTestGroupFactory();

                var results = subject.BuildMCTTestGroups(parameters);
                Assert.AreEqual(expectedGroupsCreated, results.Count());
            }
        }
    }
}
