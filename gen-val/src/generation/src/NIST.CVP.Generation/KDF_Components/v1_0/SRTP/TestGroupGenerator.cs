using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SRTP
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
                        Kdr = BitString.Zero()
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
                            Kdr = kdr
                        });
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}
