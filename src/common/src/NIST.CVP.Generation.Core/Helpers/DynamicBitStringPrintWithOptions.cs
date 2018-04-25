using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.Core.Helpers
{
    /// <summary>
    /// Instances can be used to print many <see cref="BitString"/>s with similar print options
    /// </summary>
    public class DynamicBitStringPrintWithOptions
    {
        private readonly PrintOptionBitStringNull _nullPrintOptions;
        private readonly PrintOptionBitStringEmpty _emptyPrintOptions;

        /// <summary>
        /// Constructor - takes in print options to be applied to <see cref="Print"/>
        /// </summary>
        /// <param name="nullPrintOptions"></param>
        /// <param name="emptyPrintOptions"></param>
        public DynamicBitStringPrintWithOptions(PrintOptionBitStringNull nullPrintOptions, PrintOptionBitStringEmpty emptyPrintOptions)
        {
            _nullPrintOptions = nullPrintOptions;
            _emptyPrintOptions = emptyPrintOptions;
        }

        /// <summary>
        /// Adds the <see cref="BitString"/> to the dynamic with the instance options
        /// </summary>
        /// <param name="dynamicObject">Object to add BitString too</param>
        /// <param name="label">The label of the property</param>
        /// <param name="value">The value of the property</param>
        public void AddToDynamic(dynamic dynamicObject, string label, BitString value)
        {
            DynamicHelpers.AddBitStringToDynamicWithOptions(dynamicObject, label, value, 
                _nullPrintOptions, _emptyPrintOptions);
        }
    }
}
