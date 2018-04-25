using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;

namespace NIST.CVP.Crypto.TDES
{
    public class TDESContext
    {
       
        public List<KeySchedule> Schedule{ get;}
        public TDESIVs IVs { get; set; }
        public FunctionValues Function { get; }
        public TDESContext(TDESKeys keys, FunctionValues function)
        {
            Schedule= new List<KeySchedule>();
            Function = function;
            for (int keyIdx = 0; keyIdx < keys.Keys.Count; keyIdx++)
            {
                var workingFunction = GetWorkingFunction(keyIdx);
                var keySched = new KeySchedule(keys.Keys[keyIdx], workingFunction, true);
                Schedule.Add(keySched);
            }
        }

        private FunctionValues GetWorkingFunction(int keyIdx)
        {
            var workingFunction = Function;
            if (keyIdx % 2 == 1)
            {
                if (Function == FunctionValues.Decryption)
                {
                    workingFunction = FunctionValues.Encryption;
                }
                else
                {
                    workingFunction = FunctionValues.Decryption;
                }
            }
            return workingFunction;
        }
    }
}

