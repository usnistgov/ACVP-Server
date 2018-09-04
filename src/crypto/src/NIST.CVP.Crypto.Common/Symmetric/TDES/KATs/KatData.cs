using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES.KATs
{
    public static class KatData
    {
        private static BitString _iv = new BitString("0000000000000000");

        public static string[] GetLabels()
        {
            return new []
            {
                "Permutation",
                "InversePermutation",
                "SubstitutionTable",
                "VariableKey",
                "VariableText"
            };
        }

        #region Permutation
        public static List<AlgoArrayResponse> GetPermutationData()
        {
            return null;
        }
        #endregion Permutation

        #region InversePermutation
        public static List<AlgoArrayResponse> GetInversePermutationData()
        {
            var key1 = new BitString("0101010101010101");
            var key2 = new BitString("0202020202020202");
            var key3 = new BitString("0303030303030303");

            return null;
        }
        #endregion InversePermutation

        #region SubstitutionTable
        public static List<AlgoArrayResponse> GetSubstitutionTableData()
        {
            return null;
        }
        #endregion SubstitutionTable

        #region VariableKey
        public static List<AlgoArrayResponse> GetVariableKeyData()
        {
            var key1 = new BitString("0101010101010101");
            var key2 = new BitString("0202020202020202");
            var key3 = new BitString("0303030303030303");

            return null;
        }
        #endregion VariableKey

        #region VariableText
        public static List<AlgoArrayResponse> GetVariableTextData()
        {
            var key1 = new BitString("0101010101010101");
            var key2 = new BitString("0202020202020202");
            var key3 = new BitString("0303030303030303");

            return null;
        }
        #endregion VariableText
    }
}
