using System.Dynamic;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class DynamicHelpersTests
    {
        private const string PropertyLabel = "label";

        [Test]
        public void ShouldAddNothingToDynamicWhenNull()
        {
            dynamic d = new ExpandoObject();

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, null,
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsDoubleZero
            );

            Assert.That(((ExpandoObject)d).ContainsProperty(PropertyLabel), Is.False);
        }

        [Test]
        public void ShouldAddNullEntryToDynamicWhenNull()
        {
            dynamic d = new ExpandoObject();

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, null,
                PrintOptionBitStringNull.PrintAsNull,
                PrintOptionBitStringEmpty.PrintAsDoubleZero
            );

            Assert.That(((ExpandoObject)d).ContainsProperty(PropertyLabel), Is.True, "Should contain property");
            Assert.That(d.label == null, Is.True, "Should be null");
        }

        [Test]
        public void ShouldPrintEmptyBitStringAsEmptyString()
        {
            dynamic d = new ExpandoObject();
            BitString bs = new BitString(0);

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, bs,
                PrintOptionBitStringNull.PrintAsNull,
                PrintOptionBitStringEmpty.PrintAsEmptyString
            );

            Assert.That(((ExpandoObject)d).ContainsProperty(PropertyLabel), Is.True, "Should contain property");
            Assert.That(((BitString)d.label).ToHex(), Is.EqualTo(string.Empty), "Should be null");
        }

        [Test]
        public void ShouldPrintEmptyBitStringAsDoubleZero()
        {
            dynamic d = new ExpandoObject();
            BitString bs = new BitString(0);

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, bs,
                PrintOptionBitStringNull.PrintAsNull,
                PrintOptionBitStringEmpty.PrintAsDoubleZero
            );

            Assert.That(((ExpandoObject)d).ContainsProperty(PropertyLabel), Is.True, "Should contain property");
            Assert.That((d.label).ToString(), Is.EqualTo("00"), "Should be 00");
        }

        [Test]
        public void ShouldBeBitString()
        {
            dynamic d = new ExpandoObject();
            string hex = "ABCDEF";
            BitString bs = new BitString(hex);

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, bs,
                PrintOptionBitStringNull.PrintAsNull,
                PrintOptionBitStringEmpty.PrintAsDoubleZero
            );

            Assert.That(((ExpandoObject)d).ContainsProperty(PropertyLabel), Is.True, "Should contain property");
            Assert.That(((BitString)d.label).ToHex() == hex, Is.True, $"Should equal {nameof(hex)}");
        }
    }
}
