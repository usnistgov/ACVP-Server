using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.Ed.Tests
{
    [TestFixture, LongCryptoTest]
    public class EdwardsCurveTests
    {
        [Test]
        #region Ed25519Addition
        [TestCase(Curve.Ed25519,
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            "36ab384c9f5a046c3d043b7d1833e7ac080d8e4515d7a45f83c5a14e2843ce0e", "2260cdf3092329c21da25ee8c9a21f5697390f51643851560e5f46ae6af8a3c9",
            TestName = "Add Ed25519 #1")]
        [TestCase(Curve.Ed25519,
            "36ab384c9f5a046c3d043b7d1833e7ac080d8e4515d7a45f83c5a14e2843ce0e", "2260cdf3092329c21da25ee8c9a21f5697390f51643851560e5f46ae6af8a3c9",
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            "67ae9c4a22928f491ff4ae743edac83a6343981981624886ac62485fd3f8e25c", "1267b1d177ee69aba126a18e60269ef79f16ec176724030402c3684878f5b4d4",
            TestName = "Add Ed25519 #2")]
        #endregion Ed25519Addition
        #region Ed448Addition
        [TestCase(Curve.Ed448,
            "4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e", "693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14",
            "4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e", "693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa955555555555555555555555555555555555555555555555555555555", "ae05e9634ad7048db359d6205086c2b0036ed7a035884dd7b7e36d728ad8c4b80d6565833a2a3098bbbcb2bed1cda06bdaeafbcdea9386ed",
            TestName = "Add Ed448 #1")]
        [TestCase(Curve.Ed448,
            "4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e", "693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa955555555555555555555555555555555555555555555555555555555", "ae05e9634ad7048db359d6205086c2b0036ed7a035884dd7b7e36d728ad8c4b80d6565833a2a3098bbbcb2bed1cda06bdaeafbcdea9386ed",
            "0865886b9108af6455bd64316cb6943332241b8b8cda82c7e2ba077a4a3fcfe8daa9cbf7f6271fd6e862b769465da8575728173286ff2f8f", "e005a8dbd5125cf706cbda7ad43aa6449a4a8d952356c3b9fce43c82ec4e1d58bb3a331bdb6767f0bffa9a68fed02dafb822ac13588ed6fc",
            TestName = "Add Ed448 #2")]
        #endregion Ed448Addition
        public void ShouldAddTwoPointsCorrectly(Curve curve, string axHex, string ayHex, string bxHex, string byHex, string resultXHex, string resultYHex)
        {
            var ax = LoadValue(axHex);
            var ay = LoadValue(ayHex);
            var bx = LoadValue(bxHex);
            var by = LoadValue(byHex);
            var resultx = LoadValue(resultXHex);
            var resulty = LoadValue(resultYHex);

            var a = new EdPoint(ax, ay);
            var b = new EdPoint(bx, by);
            var expectedResult = new EdPoint(resultx, resulty);

            var factory = new EdwardsCurveFactory();
            var subject = factory.GetCurve(curve);

            var result = subject.Add(a, b);

            Assert.AreEqual(expectedResult.X, result.X, "x");
            Assert.AreEqual(expectedResult.Y, result.Y, "y");
        }

        [Test]
        #region Ed25519Double
        [TestCase(Curve.Ed25519,
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            "36ab384c9f5a046c3d043b7d1833e7ac080d8e4515d7a45f83c5a14e2843ce0e", "2260cdf3092329c21da25ee8c9a21f5697390f51643851560e5f46ae6af8a3c9",
            TestName = "Double Ed25519 #1")]
        #endregion Ed25519Double
        #region Ed448Double
        [TestCase(Curve.Ed448,
            "4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e", "693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa955555555555555555555555555555555555555555555555555555555", "ae05e9634ad7048db359d6205086c2b0036ed7a035884dd7b7e36d728ad8c4b80d6565833a2a3098bbbcb2bed1cda06bdaeafbcdea9386ed",
            TestName = "Double Ed448 #1")]
        #endregion Ed448Double
        public void ShouldDoublePointsCorrectly(Curve curve, string xHex, string yHex, string resultXHex, string resultYHex)
        {
            var x = LoadValue(xHex);
            var y = LoadValue(yHex);
            var resultx = LoadValue(resultXHex);
            var resulty = LoadValue(resultYHex);

            var a = new EdPoint(x, y);
            var expectedResult = new EdPoint(resultx, resulty);

            var factory = new EdwardsCurveFactory();
            var subject = factory.GetCurve(curve);

            var result = subject.Double(a);

            Assert.AreEqual(expectedResult.X, result.X, "x");
            Assert.AreEqual(expectedResult.Y, result.Y, "y");
        }

        [Test]
        #region Ed25519Multiply
        [TestCase(Curve.Ed25519,
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            "03",
            "67ae9c4a22928f491ff4ae743edac83a6343981981624886ac62485fd3f8e25c", "1267b1d177ee69aba126a18e60269ef79f16ec176724030402c3684878f5b4d4",
            TestName = "Multiply Ed25519 #1")]
        #endregion Ed25519Multiply
        #region Ed448Multiply
        [TestCase(Curve.Ed448,
            "4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e", "693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14",
            "03",
            "0865886b9108af6455bd64316cb6943332241b8b8cda82c7e2ba077a4a3fcfe8daa9cbf7f6271fd6e862b769465da8575728173286ff2f8f", "e005a8dbd5125cf706cbda7ad43aa6449a4a8d952356c3b9fce43c82ec4e1d58bb3a331bdb6767f0bffa9a68fed02dafb822ac13588ed6fc",
            TestName = "Multiply Ed448 #1")]
        #endregion Ed448Multiply
        public void ShouldMultiplyBasisCorrectly(Curve curve, string axHex, string ayHex, string multipleHex, string resultXHex, string resultYHex)
        {
            var ax = LoadValue(axHex);
            var ay = LoadValue(ayHex);
            var multiple = LoadValue(multipleHex);
            var resultx = LoadValue(resultXHex);
            var resulty = LoadValue(resultYHex);

            var a = new EdPoint(ax, ay);
            var expectedResult = new EdPoint(resultx, resulty);

            var factory = new EdwardsCurveFactory();
            var subject = factory.GetCurve(curve);

            var result = subject.Multiply(a, multiple);

            Assert.AreEqual(expectedResult.X, result.X, "x");
            Assert.AreEqual(expectedResult.Y, result.Y, "y");
        }

        [Test]
        #region PointsOnCurve
        [TestCase(Curve.Ed25519,
            "216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a", "6666666666666666666666666666666666666666666666666666666666666658",
            true, TestName = "PointExistsOnCurve Ed25519 #1")]
        /*[TestCase(Curve.Ed25519,
            "00066499232C0FED45301404782344A374766533B60A", "0004B549A871C577A2EC6B53DE8FBCA532B9AD7E6C61",
            false, TestName = "PointExistsOnCurve b-163 #2")]
        [TestCase(Curve.Ed448,
            "00fac9dfcbac8313bb2139f1bb755fef65bc391f8b36f8f8eb7371fd558b", "01006a08a41903350678e58528bebf8a0beff867a7ca36716f7e01f81052",
            true, TestName = "PointExistsOnCurve b-233 #1")]
        [TestCase(Curve.Ed448,
            "01DA664638DA59453A816E842DC776C37D7222AF808971FC4993182804E3", "01BB7AADE0594383E663A91582D86DB7F5C5551678BA1A1BBE2F97F42069",
            false, TestName = "PointExistsOnCurve b-233 #2")]*/
        #endregion PointsOnCurve
        public void ShouldKnowIfAPointIsOnTheCurve(Curve curve, string xHex, string yHex, bool expectedResult)
        {
            var x = LoadValue(xHex);
            var y = LoadValue(yHex);

            var a = new EdPoint(x, y);
            var factory = new EdwardsCurveFactory();
            var subject = factory.GetCurve(curve);

            var result = subject.PointExistsOnCurve(a);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(0, 1, 4, 4, 4, 4)]
        [TestCase(4, 4, 4, 3, 0, 6)]
        [TestCase(5, 0, 2, 0, 0, 1)]
        [TestCase(6, 2, 1, 5, 3, 4)]
        public void ShouldAddTwoPointsCorrectly(int ax, int ay, int bx, int by, int expectedX, int expectedY)
        {
            var mattsCurve = new EdwardsCurve(Curve.Ed25519, 7, 2, 3, new EdPoint(0, 6), 12, 256, 247, 2);
            var result = mattsCurve.Add(new EdPoint(ax, ay), new EdPoint(bx, by));
            Assert.AreEqual(expectedX, (int)result.X, "x");
            Assert.AreEqual(expectedY, (int)result.Y, "y");
        }

        [Test]
        #region PointsOnCurve
        [TestCase(0, 0, false)]
        [TestCase(0, 1, true)]
        [TestCase(0, 2, false)]
        [TestCase(0, 3, false)]
        [TestCase(0, 4, false)]
        [TestCase(0, 5, false)]
        [TestCase(0, 6, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, false)]
        [TestCase(1, 2, true)]
        [TestCase(1, 3, false)]
        [TestCase(1, 4, false)]
        [TestCase(1, 5, true)]
        [TestCase(1, 6, false)]
        [TestCase(2, 0, true)]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, false)]
        [TestCase(2, 3, false)]
        [TestCase(2, 4, false)]
        [TestCase(2, 5, false)]
        [TestCase(2, 6, false)]
        [TestCase(3, 0, false)]
        [TestCase(3, 1, false)]
        [TestCase(3, 2, false)]
        [TestCase(3, 3, true)]
        [TestCase(3, 4, true)]
        [TestCase(3, 5, false)]
        [TestCase(3, 6, false)]
        [TestCase(4, 0, false)]
        [TestCase(4, 1, false)]
        [TestCase(4, 2, false)]
        [TestCase(4, 3, true)]
        [TestCase(4, 4, true)]
        [TestCase(4, 5, false)]
        [TestCase(4, 6, false)]
        [TestCase(5, 0, true)]
        [TestCase(5, 1, false)]
        [TestCase(5, 2, false)]
        [TestCase(5, 3, false)]
        [TestCase(5, 4, false)]
        [TestCase(5, 5, false)]
        [TestCase(5, 6, false)]
        [TestCase(6, 0, false)]
        [TestCase(6, 1, false)]
        [TestCase(6, 2, true)]
        [TestCase(6, 3, false)]
        [TestCase(6, 4, false)]
        [TestCase(6, 5, true)]
        [TestCase(6, 6, false)]
        #endregion PointsOnCurve
        public void ShouldKnowIfAPointIsOnMattsCurve(int x, int y, bool expectedResult)
        {
            var mattsCurve = new EdwardsCurve(Curve.Ed25519, 7, 2, 3, new EdPoint(0, 6), 12, 256, 247, 2);
            var result = mattsCurve.PointExistsOnCurve(new EdPoint(x, y));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(Curve.Ed25519, "d75a980182b10ab7d54bfed3c964073a0ee172f3daa62325af021a68f707511a")]
        [TestCase(Curve.Ed25519, "3d4017c3e843895a92b70aa74d1b7ebc9c982ccf2ec4968cc0cd55f12af4660c")]
        [TestCase(Curve.Ed25519, "fc51cd8e6218a1a38da47ed00230f0580816ed13ba3303ac5deb911548908025")]
        [TestCase(Curve.Ed25519, "278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e")]
        [TestCase(Curve.Ed25519, "ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf")]
        [TestCase(Curve.Ed448, "5fd7449b59b461fd2ce787ec616ad46a1da1342485a70e1f8a0ea75d80e96778edf124769b46c7061bd6783df1e50f6cd1fa1abeafe8256180")]
        [TestCase(Curve.Ed448, "43ba28f430cdff456ae531545f7ecd0ac834a55d9358c0372bfa0c6c6798c0866aea01eb00742802b8438ea4cb82169c235160627b4c3a9480")]
        [TestCase(Curve.Ed448, "dcea9e78f35a1bf3499a831b10b86c90aac01cd84b67a0109b55a36e9328b1e365fce161d71ce7131a543ea4cb5f7e9f1d8b00696447001400")]
        [TestCase(Curve.Ed448, "3ba16da0c6f2cc1f30187740756f5e798d6bc5fc015d7c63cc9510ee3fd44adc24d8e968b6e46e6f94d19b945361726bd75e149ef09817f580")]
        [TestCase(Curve.Ed448, "b3da079b0aa493a5772029f0467baebee5a8112d9d3a22532361da294f7bb3815c5dc59e176b4d9f381ca0938e13c6c07b174be65dfa578e80")]
        public void ShouldEncodeAndDecodeProperly(Curve curve, string encoded)
        {
            var encodedOriginal = new BitString(encoded);

            var factory = new EdwardsCurveFactory();
            var subject = factory.GetCurve(curve);

            var decoded = subject.Decode(encodedOriginal);

            Assert.IsTrue(subject.PointExistsOnCurve(decoded));

            var reEncoded = subject.Encode(decoded);

            Assert.AreEqual(encodedOriginal, reEncoded);
        }

        [Test]
        [TestCase("03")]
        public void ShouldEncodeAndDecodeProperlyMattsCurve(string encoded)
        {
            var encodedOriginal = new BitString(encoded);
            var mattsCurve = new EdwardsCurve(Curve.Ed25519, 7, 2, 3, new EdPoint(0, 6), 12, 8, 247, 2);

            var decoded = mattsCurve.Decode(encodedOriginal);

            Assert.IsTrue(mattsCurve.PointExistsOnCurve(decoded));

            Assert.AreEqual(4, (int)decoded.X);
            Assert.AreEqual(3, (int)decoded.Y);

            var reEncoded = mattsCurve.Encode(decoded);

            Assert.AreEqual(encodedOriginal, reEncoded);
        }

        [Test]
        [TestCase("2880", 3, 40)]
        [TestCase("3F80", 3, 63)]
        [TestCase("2800", 100, 40)]
        [TestCase("3F00", 100, 63)]
        public void ShouldEncodeAndDecodeProperlyMattsCurve2(string encoded, int expectedX, int expectedY)
        {
            var encodedOriginal = new BitString(encoded);
            var mattsCurve = new EdwardsCurve(Curve.Ed25519, 103, 1, 2, new EdPoint(0, 6), 12, 16, 247, 2);

            var decoded = mattsCurve.Decode(encodedOriginal);

            Assert.IsTrue(mattsCurve.PointExistsOnCurve(decoded));

            Assert.AreEqual(expectedX, (int)decoded.X);
            Assert.AreEqual(expectedY, (int)decoded.Y);

            var reEncoded = mattsCurve.Encode(decoded);

            Assert.AreEqual(encodedOriginal, reEncoded);
        }

        [Test]
        [TestCase("04", 4, 4)]
        [TestCase("09", 4, 9)]
        [TestCase("84", 9, 4)]
        [TestCase("89", 9, 9)]
        public void ShouldEncodeAndDecodeProperlyMattsCurve3(string encoded, int expectedX, int expectedY)
        {
            var encodedOriginal = new BitString(encoded);
            var mattsCurve = new EdwardsCurve(Curve.Ed25519, 13, 1, 2, new EdPoint(0, 6), 12, 8, 247, 2);

            var decoded = mattsCurve.Decode(encodedOriginal);

            Assert.IsTrue(mattsCurve.PointExistsOnCurve(decoded));

            Assert.AreEqual(expectedX, (int)decoded.X);
            Assert.AreEqual(expectedY, (int)decoded.Y);

            var reEncoded = mattsCurve.Encode(decoded);

            Assert.AreEqual(encodedOriginal, reEncoded);
        }

        private BigInteger LoadValue(string value)
        {
            var bits = new BitString(value);
            return bits.ToPositiveBigInteger();
        }
    }
}
