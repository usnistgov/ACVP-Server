using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Tests.Native.Keys
{
    [TestFixture, FastCryptoTest]
    public class XmssPrivateKeyTests
    {
        private static readonly XmssAttribute _xmssAttribute = AttributesHelper.GetXmssAttribute(XmssMode.XMSS_SHA2_10_256);

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

        [Test]
        public void WhenGivenIdxBeyondTree_ShouldThrow()
        {
            var key = KeyWithTree(0, null);
            Assert.Throws<ArgumentOutOfRangeException>(() => key.SetIdx(1 << _xmssAttribute.H));
            Assert.Throws<ArgumentOutOfRangeException>(() => key.SetIdx(-1));
        }

        private static byte[][] Nodes(int count)
        {
            // distinct, recognizable node values
            return Enumerable.Range(1, count)
                .Select(i => Enumerable.Repeat((byte)i, 32).ToArray())
                .ToArray();
        }

        private static XmssPrivateKey KeyWithTree(int x, byte[][] hashes)
        {
            var n = _xmssAttribute.N;
            return new XmssPrivateKey(_xmssAttribute, new byte[n], new byte[n], new byte[n], new byte[n], 0, x, hashes);
        }
    }
}
