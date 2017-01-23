using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public class SHA1_MCT : ISHA1_MCT
    {
        private readonly ISHA1 _iSHA1;

        public SHA1_MCT(ISHA1 iSHA1)
        {
            _iSHA1 = iSHA1;
        }

        public MCTResult MCTHash(BitString seed)
        {
            var responses = new List<AlgoArrayResponse>();

            for (var j = 0; j < 100; j++)
            {
                var MD0 = seed.GetDeepCopy();       // 3 behind current
                var MD1 = seed.GetDeepCopy();       // 2 behind current
                var MD2 = seed.GetDeepCopy();       // 1 behind current (Names match SHAVS doc)

                var incrementalResponse = new AlgoArrayResponse();

                for (var i = 3; i < 1003; i++)
                {
                    var M = MD0.ConcatenateBits(MD1).ConcatenateBits(MD2);
                    var hashResult = _iSHA1.HashMessage(M);

                    if (hashResult.Success)
                    {
                        incrementalResponse.Digest = hashResult.Digest.GetDeepCopy();
                    }
                    else
                    {
                        return new MCTResult($@"Hash outer round j = {j}, inner round i = {i} has failed.");
                    }

                    MD0 = MD1.GetDeepCopy();
                    MD1 = MD2.GetDeepCopy();
                    MD2 = incrementalResponse.Digest.GetDeepCopy();
                }

                seed = incrementalResponse.Digest.GetDeepCopy();
                responses.Add(incrementalResponse);
            }

            return new MCTResult(responses);
        }
    }
}
