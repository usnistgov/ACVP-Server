using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Common.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class IEnumerableExtensionTests
    {
        #region FirstOrNull
        [Test]
        public void FirstOrNullShouldThrowExceptionIfCollectionNull()
        {
            IEnumerable<int> items = null;

            Assert.Throws(typeof(ArgumentNullException), () => items.FirstOrNull(w => w == 1));
        }

        [Test]
        public void FirstOrNullShouldThrowExceptionIfPredicateNull()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            Assert.Throws(typeof(ArgumentNullException), () => items.FirstOrNull(null));
        }

        [Test]
        public void FirstOrNullShouldReturnNullIfItemNotFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            var result = items.FirstOrNull(w => w == 2);

            Assert.IsNull(result);
        }

        [Test]
        public void FirstOrNullShouldReturnItemWhenFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            var result = items.FirstOrNull(w => w == 1);

            Assert.AreEqual(1, result);
        }
        #endregion

        #region TryFirst
        [Test]
        public void TryFirstShouldThrowExceptionIfCollectionNull()
        {
            IEnumerable<int> items = null;

            Assert.Throws(typeof(ArgumentNullException), () => items.TryFirst(w => w == 1, out var result));
        }

        [Test]
        public void TryFirstShouldThrowExceptionIfPredicateNull()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            Assert.Throws(typeof(ArgumentNullException), () => items.TryFirst(null, out var result));
        }

        [Test]
        public void TryFirstShouldReturnFalseIfItemNotFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            var subject = items.TryFirst(w => w == 2, out var result);

            Assert.IsFalse(subject);
        }

        [Test]
        public void TryFirstShouldReturnOutDefaultIfItemNotFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            items.TryFirst(w => w == 2, out var result);

            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void TryFirstShouldReturnTrueWhenFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            var subject = items.TryFirst(w => w == 1, out var result);

            Assert.IsTrue(subject);
        }

        [Test]
        public void TryFirstShouldReturnOutItemWhenItemFound()
        {
            List<int> items = new List<int>()
            {
                 1
            };

            items.TryFirst(w => w == 1, out var result);

            Assert.AreEqual(1, result);
        }
        #endregion TryFirst
    }
}
