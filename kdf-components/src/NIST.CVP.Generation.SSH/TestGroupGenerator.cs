using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Crypto.SSH.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SSH
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var cipher in parameters.Cipher)
            {
                var modeAsEnum = EnumHelpers.GetEnumFromEnumDescription<Cipher>(cipher);
                foreach (var hash in parameters.HashAlg)
                {
                    list.Add(new TestGroup
                    {
                        Cipher = modeAsEnum,
                        HashAlg = ShaAttributes.GetHashFunctionFromName(hash)
                    });
                }
            }

            return list;
        }
    }
}
