using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CFB1
{
    public class KATDataCFB1
    {
        #region GFSBox
        public static List<BitOrientedAlgoArrayResponse> GetGFSBox128BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetGFSBox128BitKey());
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetGFSBox192BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetGFSBox192BitKey());
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetGFSBox256BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetGFSBox256BitKey());
            GFSBoxTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion GFSBox

        #region KeySBox
        public static List<BitOrientedAlgoArrayResponse> GetKeySBox128BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetKeySBox128BitKey());
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetKeySBox192BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetKeySBox192BitKey());
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetKeySBox256BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetKeySBox256BitKey());
            KeySBoxTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion KeySBox

        #region VarTxt
        public static List<BitOrientedAlgoArrayResponse> GetVarTxt128BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarTxt128BitKey());
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetVarTxt192BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarTxt192BitKey());
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetVarTxt256BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarTxt256BitKey());
            VarTxtTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion VarTxt

        #region VarTxt
        public static List<BitOrientedAlgoArrayResponse> GetVarKey128BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarKey128BitKey());
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetVarKey192BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarKey192BitKey());
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }

        public static List<BitOrientedAlgoArrayResponse> GetVarKey256BitKey()
        {
            var results = BitOrientedAlgoArrayResponse.GetDerivedFromBase(KATData.GetVarKey256BitKey());
            VarKeyTransform(results);
            AllTransform(results);
            return results;
        }
        #endregion VarTxt
        
        private static void GFSBoxTransform(List<BitOrientedAlgoArrayResponse> results)
        {
            // IV should be the pt from the original set
            results.ForEach(fe =>
            {
                fe.IV = fe.PlainText.GetDeepCopy();
            });
        }

        private static void KeySBoxTransform(List<BitOrientedAlgoArrayResponse> results)
        {
            // nothing
        }

        private static void VarKeyTransform(List<BitOrientedAlgoArrayResponse> results)
        {
            // nothing
        }

        private static void VarTxtTransform(List<BitOrientedAlgoArrayResponse> results)
        {
            // IV should be the pt from the original set
            results.ForEach(fe =>
            {
                fe.IV = fe.PlainText.GetDeepCopy();
            });
        }

        private static void AllTransform(List<BitOrientedAlgoArrayResponse> results)
        {
            // CFB1 - PT is always "0" (hex), CT is the most significant bit of the normal KAT.
            results.ForEach(fe =>
            {
                fe.PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("0");
                var mostSignificantBit = BitString.GetMostSignificantBits(1, fe.CipherText).Bits[0];
                var newBitString = mostSignificantBit ? "1" : "0";
                fe.CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(newBitString);
            });
        }
    }
}
