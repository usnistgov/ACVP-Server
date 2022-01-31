using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Common.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class TypeExtensionTests
    {
        private interface IFoo
        {
        }

        private class Foo : IFoo
        {
        }

        private class ExtendedFoo : Foo
        {
        }

        private class NotAFoo
        {
        }

        private static IEnumerable<object[]> _data = new List<object[]>()
        {
            new object[]
            {
                typeof(Foo), true
            },
            new object[]
            {
                typeof(ExtendedFoo), true
            },
            new object[]
            {
                typeof(NotAFoo), false
            },
        };

        [Test]
        [TestCaseSource(nameof(_data))]
        public void ShouldReturnTrueWhenTypeExtendsIFoo(Type type, bool expected)
        {
            Assert.AreEqual(expected, type.ImplementsInterface(typeof(IFoo)));
        }
    }
}
