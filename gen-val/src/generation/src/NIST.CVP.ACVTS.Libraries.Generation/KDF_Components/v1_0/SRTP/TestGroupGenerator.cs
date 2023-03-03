using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var aesKeyLength in parameters.AesKeyLength)
            {
                if (parameters.SupportsZeroKdr)
                {
                    list.Add(new TestGroup
                    {
                        AesKeyLength = aesKeyLength,
                        Kdr = BitString.Zero(),
                        Supports48BitSrtcpIndex = parameters.Supports48BitSrtcpIndex
                    });                        
                }

                // KdrExponent is optional
                if (parameters.KdrExponent != null)
                {
                    foreach (var kdrExponent in parameters.KdrExponent)
                    {
                        var kdr = new BitString(BigInteger.One << (kdrExponent - 1));

                        //kdr.Set(kdrExponent - 1, true);

                        list.Add(new TestGroup
                        {
                            AesKeyLength = aesKeyLength,
                            Kdr = kdr,
                            Supports48BitSrtcpIndex = parameters.Supports48BitSrtcpIndex
                        });
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}
