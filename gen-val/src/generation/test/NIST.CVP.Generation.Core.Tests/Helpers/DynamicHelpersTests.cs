using System.Dynamic;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Helpers
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

            Assert.IsFalse(((ExpandoObject)d).ContainsProperty(PropertyLabel));
        }

        [Test]
        public void ShouldAddNullEntryToDynamicWhenNull()
        {
            dynamic d = new ExpandoObject();

            DynamicHelpers.AddBitStringToDynamicWithOptions(d, PropertyLabel, null,
                PrintOptionBitStringNull.PrintAsNull,
                PrintOptionBitStringEmpty.PrintAsDoubleZero
            );

            Assert.IsTrue(((ExpandoObject)d).ContainsProperty(PropertyLabel), "Should contain property");
            Assert.IsTrue(d.label == null, "Should be null");
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

            Assert.IsTrue(((ExpandoObject)d).ContainsProperty(PropertyLabel), "Should contain property");
            Assert.AreEqual(string.Empty, ((BitString)d.label).ToHex(), "Should be null");
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

            Assert.IsTrue(((ExpandoObject)d).ContainsProperty(PropertyLabel), "Should contain property");
            Assert.AreEqual("00", (d.label).ToString(), "Should be 00");
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

            Assert.IsTrue(((ExpandoObject)d).ContainsProperty(PropertyLabel), "Should contain property");
            Assert.IsTrue(((BitString)d.label).ToHex() == hex, $"Should equal {nameof(hex)}");
        }
    }
}
