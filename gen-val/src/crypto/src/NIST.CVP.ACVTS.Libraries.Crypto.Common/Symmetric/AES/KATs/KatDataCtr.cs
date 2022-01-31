using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs
{
    public class KatDataCtr
    {
        public static List<AlgoArrayResponse> GetGfSBox(int keyLen)
        {
            switch (keyLen)
            {
                case 128:
                    return TransformKatDataToCtr(KATData.GetGFSBox128BitKey());
                case 192:
                    return TransformKatDataToCtr(KATData.GetGFSBox192BitKey());
                case 256:
                    return TransformKatDataToCtr(KATData.GetGFSBox256BitKey());
                default:
                    throw new ArgumentException($"Invalid keyLen: {keyLen}");
            }
        }

        public static List<AlgoArrayResponse> GetKeySBox(int keyLen)
        {
            switch (keyLen)
            {
                case 128:
                    return TransformKatDataToCtr(KATData.GetKeySBox128BitKey());
                case 192:
                    return TransformKatDataToCtr(KATData.GetKeySBox192BitKey());
                case 256:
                    return TransformKatDataToCtr(KATData.GetKeySBox256BitKey());
                default:
                    throw new ArgumentException($"Invalid keyLen: {keyLen}");
            }
        }

        public static List<AlgoArrayResponse> GetVarTxt(int keyLen)
        {
            switch (keyLen)
            {
                case 128:
                    return TransformKatDataToCtr(KATData.GetVarTxt128BitKey());
                case 192:
                    return TransformKatDataToCtr(KATData.GetVarTxt192BitKey());
                case 256:
                    return TransformKatDataToCtr(KATData.GetVarTxt256BitKey());
                default:
                    throw new ArgumentException($"Invalid keyLen: {keyLen}");
            }
        }

        public static List<AlgoArrayResponse> GetVarKey(int keyLen)
        {
            switch (keyLen)
            {
                case 128:
                    return TransformKatDataToCtr(KATData.GetVarKey128BitKey());
                case 192:
                    return TransformKatDataToCtr(KATData.GetVarKey192BitKey());
                case 256:
                    return TransformKatDataToCtr(KATData.GetVarKey256BitKey());
                default:
                    throw new ArgumentException($"Invalid keyLen: {keyLen}");
            }
        }

        private static List<AlgoArrayResponse> TransformKatDataToCtr(List<AlgoArrayResponse> initial)
        {
            var result = initial.Select(s => new AlgoArrayResponse
            {
                CipherText = s.CipherText,
                Key = s.Key,
                IV = s.IV,
                PlainText = s.PlainText
            }).ToList();

            // For CTR, IV and PlainText are switched
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
