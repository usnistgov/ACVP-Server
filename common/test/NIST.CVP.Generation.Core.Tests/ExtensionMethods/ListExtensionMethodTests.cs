using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class ListExtensionMethodTests
    {
        #region AddIfNotNullOrEmpty
        [Test]
        public void ShouldNotAddToListIfNull()
        {
            List<string> list = new List<string>();
            string s = null;
            list.AddIfNotNullOrEmpty(s);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ShouldNotAddToListIfEmpty()
        {
            List<string> list = new List<string>();
            string s = "";
            list.AddIfNotNullOrEmpty(s);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ShouldAddToListIfNotEmpty()
        {
            List<string> list = new List<string>();
            string s = "Not empty";
            list.AddIfNotNullOrEmpty(s);
            Assert.AreEqual(1, list.Count);
        }
        #endregion AddIfNotNullOrEmpty

        #region AddRangeIfNotNullOrEmpty
        [Test]
        public void ShouldDemonstrateErrorOnAddingNullListWithoutExtension()
        {
            List<int> list = new List<int>();
            List<int> addList = null;

            Assert.Throws(typeof(ArgumentNullException), () => list.AddRange(addList));
        }
        
        [Test]
        public void ShouldNotAddRangeToListIfNull()
        {
            List<int> list = new List<int>();
            List<int> addList = null;

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ShouldNotAddRangeToListIfEmpty()
        {
            List<int> list = new List<int>();
            List<int> addList = new List<int>();

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ShouldAddRangeToListIfNotEmpty()
        {
            List<int> list = new List<int>();
            List<int> addList = new List<int>()
            {
                1
            };

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.AreEqual(1, list.Count);
        }
        #endregion AddRangeIfNotNullOrEmpty
    }
}
