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
        /// Maps parameters to their strongly typed enum along with security strength and block size.
        /// </summary>
        public static readonly List<(string mechanism, DrbgMechanism drbgMechanism, string mode, DrbgMode drbgMode, int maxSecurityStrength, int blockSize)> Map = 
            new List<(string mechanism, DrbgMechanism drbgMechanism, string mode, DrbgMode drbgMode, int maxSecurityStrength, int blockSize)>()
        {
                ("ctrDRBG", DrbgMechanism.Counter, "AES-128", DrbgMode.AES128, 128, 128),
                ("ctrDRBG", DrbgMechanism.Counter, "AES-192", DrbgMode.AES192, 192, 128),
                ("ctrDRBG", DrbgMechanism.Counter, "AES-256", DrbgMode.AES256, 256, 128),
                ("ctrDRBG", DrbgMechanism.Counter, "TDES", DrbgMode.TDES, 112, 64),
        };
    }
}
