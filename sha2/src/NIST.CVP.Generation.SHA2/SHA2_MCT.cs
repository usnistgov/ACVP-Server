using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Generation.SHA;
using NLog;

namespace NIST.CVP.Generation.SHA2
{
    public class SHA2_MCT : ISHA2_MCT
    {
        private readonly ISHA _iSHA2;

        public SHA2_MCT(ISHA iSHA2)
        {
            _iSHA2 = iSHA2;
        }

        public MCTResult MCTHash(HashFunction hashFunction, BitString seed)
        {
            var responses = new List<AlgoArrayResponse>();
            int i = 0, j = 0;
            BitString M = null;

            try
            {
                for (j = 0; j < 100; j++)
                {
                    var MD0 = seed.GetDeepCopy();       // 3 behind current
                    var MD1 = seed.GetDeepCopy();       // 2 behind current
                    var MD2 = seed.GetDeepCopy();       // 1 behind current (Names match SHAVS doc)

                    var incrementalResponse = new AlgoArrayResponse();
                    incrementalResponse.Message =
                        BitString.ConcatenateBits(
                            BitString.ConcatenateBits(MD0, MD1), MD2);

                    BitString outputDigest = null;

                    for (i = 0; i < 1000; i++)
                    {
                        //BitString M = null;
                        M = BitString.ConcatenateBits(MD0, MD1);
                        M = BitString.ConcatenateBits(M, MD2);

                        var hashResult = _iSHA2.HashMessage(hashFunction, M);

                        if (!hashResult.Success)
                        {
                            throw new Exception("Hash Failed.");
                        }

                        outputDigest = hashResult.Digest;

                        MD0 = MD1.GetDeepCopy();
                        MD1 = MD2.GetDeepCopy();
                        MD2 = outputDigest.GetDeepCopy();
                    }

                    incrementalResponse.Digest = outputDigest.GetDeepCopy();
                    seed = outputDigest.GetDeepCopy();
                    responses.Add(incrementalResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"M = {M.ToHex()}");
                ThisLogger.Debug($"Hash outer round j = {j}, inner round i = {i}.");
                ThisLogger.Error(ex);
                return new MCTResult(ex.Message);
            }

            return new MCTResult(responses);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
