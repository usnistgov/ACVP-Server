using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs
{
    public class KATDataCFB1
    {
        #region GFSBox
        public static List<AlgoArrayResponse> GetGFSBox128BitKey()
        {
            var results = KATData.GetGFSBox128BitKey();
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetGFSBox192BitKey()
        {
            var results = KATData.GetGFSBox192BitKey();
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetGFSBox256BitKey()
        {
            var results = KATData.GetGFSBox256BitKey();
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion GFSBox

        #region KeySBox
        public static List<AlgoArrayResponse> GetKeySBox128BitKey()
        {
            var results = KATData.GetKeySBox128BitKey();
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetKeySBox192BitKey()
        {
            var results = KATData.GetKeySBox192BitKey();
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetKeySBox256BitKey()
        {
            var results = KATData.GetKeySBox256BitKey();
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion KeySBox

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarTxt128BitKey()
        {
            var results = KATData.GetVarTxt128BitKey();
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetVarTxt192BitKey()
        {
            var results = KATData.GetVarTxt192BitKey();
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetVarTxt256BitKey()
        {
            var results = KATData.GetVarTxt256BitKey();
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion VarTxt

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarKey128BitKey()
        {
            var results = KATData.GetVarKey128BitKey();
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetVarKey192BitKey()
        {
            var results = KATData.GetVarKey192BitKey();
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<AlgoArrayResponse> GetVarKey256BitKey()
        {
            var results = KATData.GetVarKey256BitKey();
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion VarTxt

        private static void GFSBoxTransform(List<AlgoArrayResponse> results)
        {
            // IV should be the pt from the original set
            results.ForEach(fe =>
            {
                fe.IV = fe.PlainText.GetDeepCopy();
            });
        }

        private static void KeySBoxTransform(List<AlgoArrayResponse> results)
        {
            // nothing
        }

        private static void VarKeyTransform(List<AlgoArrayResponse> results)
        {
            // nothing
        }

        private static void VarTxtTransform(List<AlgoArrayResponse> results)
        {
            // IV should be the pt from the original set
            results.ForEach(fe =>
            {
                fe.IV = fe.PlainText.GetDeepCopy();
            });
        }

        private static void AllTransform(List<AlgoArrayResponse> results)
        {
            // CFB1 - PT is always "0" (hex), CT is the most significant bit of the normal KAT.
            results.ForEach(fe =>
            {
                fe.PlainText = new BitString("00", 1);
                fe.CipherText = BitString.GetMostSignificantBits(1, fe.CipherText);
            });
        }
    }
}
