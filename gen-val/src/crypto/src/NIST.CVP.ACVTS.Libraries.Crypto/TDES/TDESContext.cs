using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES
{
    public class TDESContext
    {

        public List<KeySchedule> Schedule { get; }
        public TDESIVs IVs { get; set; }
        public BlockCipherDirections Function { get; }
        public TDESContext(TDESKeys keys, BlockCipherDirections function)
        {
            Schedule = new List<KeySchedule>();
            Function = function;
            for (int keyIdx = 0; keyIdx < keys.Keys.Count; keyIdx++)
            {
                var workingFunction = GetWorkingFunction(keyIdx);
                var keySched = new KeySchedule(keys.Keys[keyIdx], workingFunction, true);
                Schedule.Add(keySched);
            }
        }

        private BlockCipherDirections GetWorkingFunction(int keyIdx)
        {
            var workingFunction = Function;
            if (keyIdx % 2 == 1)
            {
                if (Function == BlockCipherDirections.Decrypt)
                {
                    workingFunction = BlockCipherDirections.Encrypt;
                }
                else
                {
                    workingFunction = BlockCipherDirections.Decrypt;
                }
            }
            return workingFunction;
        }
    }
}

