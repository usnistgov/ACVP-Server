using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_OFB
{
    public class KATDataOFB
    {
        static BitString _iv = new BitString("00000000000000000000000000000000");

        #region GFSBox
        public static List<AlgoArrayResponse> GetGFSBox128BitKey()
        {
            var initial = KATData.GetGFSBox128BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetGFSBox192BitKey()
        {
            var initial = KATData.GetGFSBox192BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetGFSBox256BitKey()
        {
            var initial = KATData.GetGFSBox256BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }
        #endregion GFSBox

        #region KeySBox
        public static List<AlgoArrayResponse> GetKeySBox128BitKey()
        {
            var initial = KATData.GetKeySBox128BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetKeySBox192BitKey()
        {
            var initial = KATData.GetKeySBox192BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetKeySBox256BitKey()
        {
            var initial = KATData.GetKeySBox256BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }
        #endregion KeySBox

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarTxt128BitKey()
        {
            var initial = KATData.GetVarTxt128BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetVarTxt192BitKey()
        {
            var initial = KATData.GetVarTxt192BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetVarTxt256BitKey()
        {
            var initial = KATData.GetVarTxt256BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }
        #endregion VarTxt

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarKey128BitKey()
        {
            var initial = KATData.GetVarKey128BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetVarKey192BitKey()
        {
            var initial = KATData.GetVarKey192BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }

        public static List<AlgoArrayResponse> GetVarKey256BitKey()
        {
            var initial = KATData.GetVarKey256BitKey();
            return TransformKATDataToKATDataOFb(initial);
        }
        #endregion VarTxt
        
        private static List<AlgoArrayResponse> TransformKATDataToKATDataOFb(List<AlgoArrayResponse> initial)
        {
            var result = initial.Select(s => new AlgoArrayResponse()
            {
                CipherText = s.CipherText,
                Key = s.Key,
                IV = s.IV,
                PlainText = s.PlainText
            }).ToList();

            // For OFB, IV and PlainText are switched
            result.ForEach(fe =>
            {
                var oldPt = fe.PlainText.GetDeepCopy();
                var oldIv = fe.IV.GetDeepCopy();

                fe.PlainText = oldIv;
                fe.IV = oldPt;
            });

            return result;
        }
    }
}
