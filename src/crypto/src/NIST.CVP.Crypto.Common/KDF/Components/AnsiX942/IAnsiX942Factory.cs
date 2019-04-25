﻿using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;

namespace NIST.CVP.Crypto.Common.KDF.Components.AnsiX942
{
    public interface IAnsiX942Factory
    {
        IAnsiX942 GetInstance(AnsiX942Types type, HashFunction hashFunction);
    }
}
