using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SRTP
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var aesKeyLength in parameters.AesKeyLength)
            {
                if (parameters.SupportsZeroKdr)
                {
                    list.Add(new TestGroup
                    {
                        AesKeyLength = aesKeyLength,
                        Kdr = BitString.Zero()
                    });
                }

                foreach (var kdrExponent in parameters.KdrExponent)
                {
                    var kdr = new BitString(BigInteger.One << (kdrExponent - 1));
                    
                    //kdr.Set(kdrExponent - 1, true);

                    list.Add(new TestGroup
                    {
                        AesKeyLength = aesKeyLength,
                        Kdr = kdr
                    });
                }
            }

            return list;
        }
    }
}
