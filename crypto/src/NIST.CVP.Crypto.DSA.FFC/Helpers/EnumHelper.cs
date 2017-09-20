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

        public static string GGenModeToString(GeneratorGenMode gGenMode)
        {
            switch (gGenMode)
            {
                case GeneratorGenMode.Canonical:
                    return "canonical";
                case GeneratorGenMode.Unverifiable:
                    return "unverifiable";
            }

            throw new ArgumentOutOfRangeException("Not a valid GeneratorGenMode");
        }

        public static string PQGenModeToString(PrimeGenMode pQGenMode)
        {
            switch (pQGenMode)
            {
                case PrimeGenMode.Probable:
                    return "probable";
                case PrimeGenMode.Provable:
                    return "provable";
            }

            throw new ArgumentOutOfRangeException("Not a valid PrimeGenMode");
        }
    }
}
