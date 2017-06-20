using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public static class TestCaseGeneratorHelper
    {
        /// <summary>
        /// Gets an e value for the TestCaseGenerator
        /// </summary>
        /// <param name="group"></param>
        /// <param name="rand"></param>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Exclusive</param>
        /// <returns></returns>
        public static BigInteger GetEValue(TestGroup group, IRandom800_90 rand, BigInteger min, BigInteger max)
        {
            if (group.PubExp == PubExpModes.FIXED)
            {
                return group.FixedPubExp.ToPositiveBigInteger();
            }
            else
            {
                BigInteger e;
                do
                {
                    e = rand.GetRandomBigInteger(min, max);
                    if (e.IsEven)
                    {
                        e++;
                    }
                } while (e >= max);      // sanity check

                return e;
            }
        }

        /// <summary>
        /// Gets a seed value for the TestCaseGenerator
        /// </summary>
        /// <param name="group"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static BitString GetSeed(TestGroup group, IRandom800_90 rand)
        {
            var security_strength = 0;
            if (group.Modulo == 2048)
            {
                security_strength = 112;
            }
            else if (group.Modulo == 3072)
            {
                security_strength = 128;
            }

            return rand.GetRandomBitString(2 * security_strength);
        }

        /// <summary>
        /// Gets bitlen values for the TestCaseGenerator
        /// </summary>
        /// <param name="group"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static int[] GetBitlens(TestGroup group, IRandom800_90 rand)
        {
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if (group.Modulo == 2048)
            {
                min_single = 140 + 1;
                max_both = 494;
            }
            else if (group.Modulo == 3072)
            {
                min_single = 170 + 1;
                max_both = 750;
            }

            bitlens[0] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }

        /// <summary>
        /// Gets an empty test case (with optional e value) for the TestCaseGenerator
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static TestCase GetEmptyTestCase(TestGroup group)
        {
            var testCase = new TestCase();
            if (group.PubExp == PubExpModes.FIXED)
            {
                testCase.Key = new KeyPair {PubKey = new PublicKey {E = group.FixedPubExp.ToPositiveBigInteger()}};
            }

            testCase.Deferred = true;
            return testCase;
        }
    }
}
