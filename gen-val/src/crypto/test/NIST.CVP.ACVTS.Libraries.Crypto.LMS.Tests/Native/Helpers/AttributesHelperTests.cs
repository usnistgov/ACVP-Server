using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native.Helpers
{
    [TestFixture]
    public class AttributesHelperTests
    {
        [Test]
        [TestCase(null, LmOtsMode.Invalid)]
        [TestCase(new byte[] { }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 0 }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 1 }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 1, 0, 0, 0 }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 0, 0, 0, 25 }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 1, 0, 0, 1 }, LmOtsMode.Invalid)]
        [TestCase(new byte[] { 0, 0, 0, 1 }, LmOtsMode.LMOTS_SHA256_N32_W1)]
        [TestCase(new byte[] { 0, 0, 0, 2 }, LmOtsMode.LMOTS_SHA256_N32_W2)]
        [TestCase(new byte[] { 0, 0, 0, 3 }, LmOtsMode.LMOTS_SHA256_N32_W4)]
        [TestCase(new byte[] { 0, 0, 0, 4 }, LmOtsMode.LMOTS_SHA256_N32_W8)]

        [TestCase(new byte[] { 0, 0, 0, 5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
        [TestCase(new byte[] { 0, 0, 0, 6 }, LmOtsMode.LMOTS_SHA256_N24_W2)]
        [TestCase(new byte[] { 0, 0, 0, 7 }, LmOtsMode.LMOTS_SHA256_N24_W4)]
        [TestCase(new byte[] { 0, 0, 0, 8 }, LmOtsMode.LMOTS_SHA256_N24_W8)]

        [TestCase(new byte[] { 0, 0, 0, 9 }, LmOtsMode.LMOTS_SHAKE_N32_W1)]
        [TestCase(new byte[] { 0, 0, 0, 10 }, LmOtsMode.LMOTS_SHAKE_N32_W2)]
        [TestCase(new byte[] { 0, 0, 0, 11 }, LmOtsMode.LMOTS_SHAKE_N32_W4)]
        [TestCase(new byte[] { 0, 0, 0, 12 }, LmOtsMode.LMOTS_SHAKE_N32_W8)]

        [TestCase(new byte[] { 0, 0, 0, 13 }, LmOtsMode.LMOTS_SHAKE_N24_W1)]
        [TestCase(new byte[] { 0, 0, 0, 14 }, LmOtsMode.LMOTS_SHAKE_N24_W2)]
        [TestCase(new byte[] { 0, 0, 0, 15 }, LmOtsMode.LMOTS_SHAKE_N24_W4)]
        [TestCase(new byte[] { 0, 0, 0, 16 }, LmOtsMode.LMOTS_SHAKE_N24_W8)]
        public void WhenGivenByteArray_ShouldReturnCorrectLmOtsMode(byte[] typeCode, LmOtsMode expectedMode)
        {
            var result = AttributesHelper.GetLmOtsModeFromTypeCode(typeCode);

            Assert.That(result, Is.EqualTo(expectedMode));
        }

        [Test]
        [TestCase(null, LmsMode.Invalid)]
        [TestCase(new byte[] { }, LmsMode.Invalid)]
        [TestCase(new byte[] { 0 }, LmsMode.Invalid)]
        [TestCase(new byte[] { 1 }, LmsMode.Invalid)]
        [TestCase(new byte[] { 1, 0, 0, 0 }, LmsMode.Invalid)]
        [TestCase(new byte[] { 0, 0, 0, 10 }, LmsMode.LMS_SHA256_M24_H5)]
        [TestCase(new byte[] { 0, 0, 0, 11 }, LmsMode.LMS_SHA256_M24_H10)]
        [TestCase(new byte[] { 0, 0, 0, 12 }, LmsMode.LMS_SHA256_M24_H15)]
        [TestCase(new byte[] { 0, 0, 0, 13 }, LmsMode.LMS_SHA256_M24_H20)]
        [TestCase(new byte[] { 0, 0, 0, 14 }, LmsMode.LMS_SHA256_M24_H25)]
        [TestCase(new byte[] { 0, 0, 0, 5 }, LmsMode.LMS_SHA256_M32_H5)]
        [TestCase(new byte[] { 0, 0, 0, 6 }, LmsMode.LMS_SHA256_M32_H10)]
        [TestCase(new byte[] { 0, 0, 0, 7 }, LmsMode.LMS_SHA256_M32_H15)]
        [TestCase(new byte[] { 0, 0, 0, 8 }, LmsMode.LMS_SHA256_M32_H20)]
        [TestCase(new byte[] { 0, 0, 0, 9 }, LmsMode.LMS_SHA256_M32_H25)]
        [TestCase(new byte[] { 0, 0, 0, 20 }, LmsMode.LMS_SHAKE_M24_H5)]
        [TestCase(new byte[] { 0, 0, 0, 21 }, LmsMode.LMS_SHAKE_M24_H10)]
        [TestCase(new byte[] { 0, 0, 0, 22 }, LmsMode.LMS_SHAKE_M24_H15)]
        [TestCase(new byte[] { 0, 0, 0, 23 }, LmsMode.LMS_SHAKE_M24_H20)]
        [TestCase(new byte[] { 0, 0, 0, 24 }, LmsMode.LMS_SHAKE_M24_H25)]
        [TestCase(new byte[] { 0, 0, 0, 15 }, LmsMode.LMS_SHAKE_M32_H5)]
        [TestCase(new byte[] { 0, 0, 0, 16 }, LmsMode.LMS_SHAKE_M32_H10)]
        [TestCase(new byte[] { 0, 0, 0, 17 }, LmsMode.LMS_SHAKE_M32_H15)]
        [TestCase(new byte[] { 0, 0, 0, 18 }, LmsMode.LMS_SHAKE_M32_H20)]
        [TestCase(new byte[] { 0, 0, 0, 19 }, LmsMode.LMS_SHAKE_M32_H25)]
        public void WhenGivenByteArray_ShouldReturnCorrectLmsMode(byte[] typeCode, LmsMode expectedMode)
        {
            var result = AttributesHelper.GetLmsModeFromTypeCode(typeCode);

            Assert.That(result, Is.EqualTo(expectedMode));
        }

        private static readonly IEnumerable<object> _mappingTests = new List<object>()
        {
            new object[]
            {
                "one function/output length, one LMS/LM-OTS mode",
                new[] { LmsMode.LMS_SHA256_M24_H5 },
                new[] { LmOtsMode.LMOTS_SHA256_N24_W1 },
                new List<MappedLmsLmOtsModesToFunctionOutputLength>()
                {
                    new(ModeValues.SHA2, 24,
                        new List<LmsMode>() { LmsMode.LMS_SHA256_M24_H5 },
                        new List<LmOtsMode>() {LmOtsMode.LMOTS_SHA256_N24_W1})
                }
            },
            new object[]
            {
                "two functions/output lengths with multiple modes for each",
                new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H15, LmsMode.LMS_SHAKE_M24_H5, LmsMode.LMS_SHAKE_M32_H5 },
                new[] { LmOtsMode.LMOTS_SHA256_N24_W1, LmOtsMode.LMOTS_SHA256_N24_W2, LmOtsMode.LMOTS_SHAKE_N24_W1, LmOtsMode.LMOTS_SHAKE_N32_W1 },
                new List<MappedLmsLmOtsModesToFunctionOutputLength>()
                {
                    new(ModeValues.SHA2, 24,
                        new List<LmsMode>() { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H15 },
                        new List<LmOtsMode>() { LmOtsMode.LMOTS_SHA256_N24_W1, LmOtsMode.LMOTS_SHA256_N24_W2 }),
                    new(ModeValues.SHAKE, 24,
                        new List<LmsMode>() { LmsMode.LMS_SHAKE_M24_H5 },
                        new List<LmOtsMode>() { LmOtsMode.LMOTS_SHAKE_N24_W1 }),
                    new(ModeValues.SHAKE, 32,
                        new List<LmsMode>() { LmsMode.LMS_SHAKE_M32_H5 },
                        new List<LmOtsMode>() { LmOtsMode.LMOTS_SHAKE_N32_W1 }),
                }
            },
        };

        [Test]
        [TestCaseSource(nameof(_mappingTests))]
        public void WhenGivenLmsLmOtsModes_ShouldReturnProperlyWrappedObjectOfFunctionAndOutputLength(string label,
            LmsMode[] lmsModes, LmOtsMode[] lmOtsModes, List<MappedLmsLmOtsModesToFunctionOutputLength> expectedOutput)
        {
            var map = AttributesHelper.GetMappedLmsLmOtsModesToFunctionOutputLength(lmsModes, lmOtsModes);

            expectedOutput = OrderMapping(expectedOutput);
            map = OrderMapping(map);

            Assert.That(expectedOutput.JsonCompare(map), Is.True);
        }

        private List<MappedLmsLmOtsModesToFunctionOutputLength> OrderMapping(
            List<MappedLmsLmOtsModesToFunctionOutputLength> preOrder)
        {
            return preOrder
                .OrderBy(ob => ob.OutputLength)
                .ThenBy(tb => tb.Mode)
                .ThenBy(tb => tb.LmsModes)
                .ThenBy(tb => tb.LmOtsModes)
                .ToList();
        }
    }
}
