using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.Core.Helpers
{
    /// <summary>
    /// Provides some helper functions related to dynamics
    /// </summary>
    public static class DynamicHelpers
    {
        /// <summary>
        /// Helper for adding a <see cref="BitString"/> to a dynamic with options
        /// </summary>
        /// <param name="dynamicObject">The dynamic for the <see cref="BitString"/> to be added too (or not depending on content/options)</param>
        /// <param name="label">The label the <see cref="BitString"/> should be applied to in the dynamic</param>
        /// <param name="value">The <see cref="BitString"/> instance to print</param>
        /// <param name="nullPrintOption">Indicates how the <see cref="BitString"/> should be printed when null</param>
        /// <param name="emptyPrintOption">Indicates how the <see cref="BitString"/> should be printed when empty (zero length)</param>
        public static void AddBitStringToDynamicWithOptions(dynamic dynamicObject, string label, BitString value, PrintOptionBitStringNull nullPrintOption = PrintOptionBitStringNull.DoNotPrintProperty, PrintOptionBitStringEmpty emptyPrintOption = PrintOptionBitStringEmpty.PrintAsDoubleZero)
        {
            if (value == null)
            {
                if (nullPrintOption == PrintOptionBitStringNull.PrintAsNull)
                { 
                    ((IDictionary<string, object>)dynamicObject).Add(label, null);
                }

                // Handles "DoNotPrintProperty" as "do nothing" as well as return for PrintAsNull option
                return;
            }

            // When 0 bit BitString, and PrintAsDoubleZero, print as "00"
            if (value.BitLength == 0 && emptyPrintOption == PrintOptionBitStringEmpty.PrintAsDoubleZero)
            {
                ((IDictionary<string, object>) dynamicObject).Add(label, "00");
                return;
            }

            // When 0 bit BitString, and DoNotPrintProperty, don't print the property
            if (value.BitLength == 0 && emptyPrintOption == PrintOptionBitStringEmpty.DoNotPrintProperty)
            {
                return;
            }

            // Default behavior, empty bitstrings print as ""
            ((IDictionary<string, object>)dynamicObject).Add(label, value);
        }

        /// <summary>
        /// Adds a biginteger to the dynamic when it is not 0
        /// </summary>
        /// <param name="dynamicObject">The object to add the biginteger to</param>
        /// <param name="label">The label to add to the dynamic object</param>
        /// <param name="value">The value to add</param>
        public static void AddBigIntegerToDynamicWhenNotZero(dynamic dynamicObject, string label, BigInteger value)
        {
            if (value == BigInteger.Zero)
            {
                return;
            }

            ((IDictionary<string, object>)dynamicObject).Add(label, value);
        }

        /// <summary>
        /// Adds an int to the dynamic when it is not 0
        /// </summary>
        /// <param name="dynamicObject">The object to add the int to</param>
        /// <param name="label">The label to add to the dynamic object</param>
        /// <param name="value">The value to add</param>
        public static void AddIntegerToDynamicWhenNotZero(dynamic dynamicObject, string label, int value)
        {
            if (value == 0)
            {
                return;
            }

            ((IDictionary<string, object>)dynamicObject).Add(label, value);
        }
    }
}
