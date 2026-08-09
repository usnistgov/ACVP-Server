using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native.Keys
{
    [TestFixture, FastCryptoTest]
    public class LmsPrivateKeyTests
    {
        private static readonly LmsAttribute _lmsAttribute = AttributesHelper.GetLmsAttribute(LmsMode.LMS_SHA256_M24_H5);
        private static readonly LmOtsAttribute _lmOtsAttribute = AttributesHelper.GetLmOtsAttribute(LmOtsMode.LMOTS_SHA256_N24_W1);

        [Test]
        public void WhenGivenEitherPrecomputedTreeShape_ShouldExposeIdenticalNodes()
        {
            const int x = 2;
            var nodeCount = (2 << x) - 1;
            var nodes = Nodes(nodeCount);

            // compact shape: values only, hashes[0] is T[1]
            var compact = KeyWithTree(x, nodes);

            // one-indexed shape: slot 0 unused
            var oneIndexed = KeyWithTree(x, new byte[][] { Array.Empty<byte>() }.Concat(nodes).ToArray());

            Assert.Multiple(() =>
            {
                for (var r = 1; r <= nodeCount; r++)
                {
                    Assert.That(compact.HasPrecomputedHash(r), Is.True, $"compact node {r} present");
                    Assert.That(oneIndexed.HasPrecomputedHash(r), Is.True, $"one-indexed node {r} present");
                    Assert.That(compact.GetTreeNodeAtIndex(r), Is.EqualTo(oneIndexed.GetTreeNodeAtIndex(r)),
                        $"node {r} equal across shapes");
                }

                Assert.That(compact.HasPrecomputedHash(nodeCount + 1), Is.False, "beyond stored range");
            });
        }

        [Test]
        public void WhenGivenIncompleteTree_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => KeyWithTree(2, Nodes(3)));
        }

        private static byte[][] Nodes(int count)
        {
            // distinct, recognizable node values
            return Enumerable.Range(1, count)
                .Select(i => Enumerable.Repeat((byte)i, 24).ToArray())
                .ToArray();
        }

        private static LmsPrivateKey KeyWithTree(int x, byte[][] hashes)
        {
            return new LmsPrivateKey(_lmsAttribute, _lmOtsAttribute, new byte[16], new byte[24], 0, x, hashes);
        }
    }
}
