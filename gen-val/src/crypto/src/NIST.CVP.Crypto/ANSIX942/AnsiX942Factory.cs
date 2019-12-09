using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;
using System;

namespace NIST.CVP.Crypto.ANSIX942
{
    public class AnsiX942Factory : IAnsiX942Factory
    {
        private readonly IShaFactory _shaFactory;

        public AnsiX942Factory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IAnsiX942 GetInstance(AnsiX942Types type, HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            switch (type)
            {
                case AnsiX942Types.Concat:
                    return new AnsiX942Concat(sha);

                case AnsiX942Types.Der:
                    return new AnsiX942Der(sha);

                default:
                    throw new Exception("No ANSI x9.42 KDF type specified");
            }
        }
    }
}
