﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ANSIX963
{
    public class AnsiX963Factory : IAnsiX963Factory
    {
        private readonly IShaFactory _shaFactory;

        public AnsiX963Factory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IAnsiX963 GetInstance(HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            return new AnsiX963(sha);
        }
    }
}
