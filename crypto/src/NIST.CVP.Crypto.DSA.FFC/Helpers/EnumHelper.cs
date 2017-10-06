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

        public static string ReasonToString(PQFailureReasons reason)
        {
            switch (reason)
            {
                case PQFailureReasons.ModifyP:
                    return "modify p";

                case PQFailureReasons.ModifyQ:
                    return "modify q";

                case PQFailureReasons.ModifySeed:
                    return "modify seed";

                case PQFailureReasons.None:
                    return "none";
            }

            throw new ArgumentOutOfRangeException("Not a valid FailureReason");
        }

        public static string ReasonToString(GFailureReasons reason)
        {
            switch (reason)
            {
                case GFailureReasons.ModifyG:
                    return "modify g";

                case GFailureReasons.None:
                    return "none";
            }

            throw new ArgumentOutOfRangeException("Not a valid FailureReason");
        }

        public static PQFailureReasons StringToPQReason(string reason)
        {
            switch (reason)
            {
                case "none":
                    return PQFailureReasons.None;

                case "modify p":
                    return PQFailureReasons.ModifyP;

                case "modify q":
                    return PQFailureReasons.ModifyQ;

                case "modify seed":
                    return PQFailureReasons.ModifySeed;
            }

            throw new ArgumentOutOfRangeException("Not a valid FailureReason");
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

        public static string SigFailureReasonToString(SigFailureReasons reason)
        {
            switch (reason)
            {
                case SigFailureReasons.None:
                    return "none";

                case SigFailureReasons.ModifyMessage:
                    return "modify message";

                case SigFailureReasons.ModifyKey:
                    return "modify public key";

                case SigFailureReasons.ModifyR:
                    return "modify r";

                case SigFailureReasons.ModifyS:
                    return "modify s";
            }

            throw new ArgumentOutOfRangeException("Not a valid SigFailureReason");
        }

        public static SigFailureReasons StringToSigFailureReason(string reason)
        {
            switch (reason)
            {
                case "none":
                    return SigFailureReasons.None;

                case "modify message":
                    return SigFailureReasons.ModifyMessage;

                case "modify public key":
                    return SigFailureReasons.ModifyKey;

                case "modify r":
                    return SigFailureReasons.ModifyR;

                case "modify s":
                    return SigFailureReasons.ModifyS;
            }

            throw new ArgumentOutOfRangeException("Not a valid SigFailureReason");
        }
    }
}
