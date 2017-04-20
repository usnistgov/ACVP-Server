using System;
using System.Collections.Generic;
using System.Linq;
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
        
            // When 0 bit BitString, and PrintAsEmptyBitstring, print as ""
            if (value.BitLength == 0 && emptyPrintOption == PrintOptionBitStringEmpty.PrintAsEmptyString)
            {
                ((IDictionary<string, object>) dynamicObject).Add(label, string.Empty);
                return;
            }

            // When 0 bit BitString, and DoNotPrintProperty, don't print the property
            if (value.BitLength == 0 && emptyPrintOption == PrintOptionBitStringEmpty.DoNotPrintProperty)
            {
                return;
            }

            ((IDictionary<string, object>)dynamicObject).Add(label, value);
        }
    }
}
