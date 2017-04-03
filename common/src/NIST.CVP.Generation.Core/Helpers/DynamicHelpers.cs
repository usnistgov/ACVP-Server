using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.Core.Helpers
{
    public static class DynamicHelpers
    {
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

            ((IDictionary<string, object>)dynamicObject).Add(label, value);
        }
    }
}
