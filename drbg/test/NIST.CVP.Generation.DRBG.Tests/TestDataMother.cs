using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            List<TestGroup> list = new EditableList<TestGroup>();

            for (int i = 0; i < groups; i++)
            {
                TestGroup tg = new TestGroup()
                {
                    DrbgParameters = new DrbgParameters()
                    {
                        NonceLen = 1,
                        AdditionalInputLen = 2,
                        EntropyInputLen = 3,
                        ReturnedBitsLen = 4,
                        PersoStringLen = 5,
                        DerFuncEnabled = true,
                        Mode = DrbgMode.AES128,
                        SecurityStrength = 128,
                        PredResistanceEnabled = true,
                        ReseedImplemented = true,
                        Mechanism = DrbgMechanism.Counter
                    }
                };
                tg.Tests = new List<ITestCase>()
                {
                    new TestCase()
                    {
                        EntropyInput = new BitString("11"),
                        Nonce = new BitString("22"),
                        PersoString = new BitString("33"),
                        OtherInput = new List<OtherInfo>()
                        {
                            new OtherInfo()
                            {
                                AdditionalInput = new BitString("44"),
                                EntropyInput = new BitString("55")
                            }
                        },
                        ReturnedBits = new BitString("42")
                    }
                };
                list.Add(tg);
            }

            return list;
        }
    }
}