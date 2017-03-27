using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public class SHA3 : ISHA3
    {
        private BitString _message;

        public HashResult HashMessage(BitString message)
        {
            Init();
            Update(message);
            var digest = Final();
            return new HashResult(digest);
        }

        private void Init()
        {
            
        }

        private void Update(BitString newContent)
        {
            
        }

        private BitString Final()
        {
            return KeccakInternals.Keccak(_message, 0, 0, false);
        }
    }
}
