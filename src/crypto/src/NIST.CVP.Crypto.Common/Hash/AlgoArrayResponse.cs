using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class AlgoArrayResponse
    {
        // Can Tuple, Customization, etc. just be added to this file without affecting the other solutions?
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public int DigestLength => Digest.BitLength;

        // This works because probablistically the MCT will not have 'collisions' at all of these values?
        // Used for TupleHash
        public void SetMessageFromTuple(IEnumerable<BitString> tuple)
        {
            var message = new BitString(0);
            foreach (var bitstring in tuple)
            {
                message = BitString.ConcatenateBits(message, bitstring);
            }
            Message = message;
        }
    }
}
