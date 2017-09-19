using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC.Helpers
{
    public static class EnumHelper
    {
        public static PrimeGenMode StringToPQGenMode(string value)
        {
            switch (value.ToLower())
            {
                case "probable":
                    return PrimeGenMode.Probable;
                case "provable":
                    return PrimeGenMode.Provable;
            }

            throw new ArgumentOutOfRangeException($"Not a valid PrimeGenMode: {value}");
        }

        public static GeneratorGenMode StringToGGenMode(string value)
        {
            switch (value.ToLower())
            {
                case "unverifiable":
                    return GeneratorGenMode.Unverifiable;
                case "canonical":
                    return GeneratorGenMode.Canonical;
            }

            throw new ArgumentOutOfRangeException($"Not a valid GeneratorGenMode: {value}");
        }
    }
}
