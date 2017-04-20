using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.DRBG.Enums;

namespace NIST.CVP.Generation.DRBG
{
    /// <summary>
    /// Used to help map between the ACVP specification (and subsequent json files)
    /// To their strongly typed implementation on the gev/vals
    /// </summary>
    public class DrbgSpecToDomainMapping
    {
        /// <summary>
        /// algorithm, DrbgMechanism, mode, DrbgMode, maxSecurityStrength, blockSize
        /// TODO Update this with strongly typed tuple implementation from C# 7
        /// </summary>
        public static readonly List<Tuple<string, DrbgMechanism, string, DrbgMode, int, int>> Map =
            new List<Tuple<string, DrbgMechanism, string, DrbgMode, int, int>>()
            {
                new Tuple<string, DrbgMechanism, string, DrbgMode, int, int>("ctrDRBG", DrbgMechanism.Counter, "AES-128",
                    DrbgMode.AES128, 128, 128),
                new Tuple<string, DrbgMechanism, string, DrbgMode, int, int>("ctrDRBG", DrbgMechanism.Counter, "AES-192",
                    DrbgMode.AES192, 192, 128),
                new Tuple<string, DrbgMechanism, string, DrbgMode, int, int>("ctrDRBG", DrbgMechanism.Counter, "AES-256",
                    DrbgMode.AES256, 256, 128)
            };
    }
}
