using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.RSA;

namespace LCAVPCore.AlgorithmEvaluators.RSA
{
	public class RSASigVer_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSASigVer_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSASigVer_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//SigVer931
				if (Options.GetValue("FIPS186_3SigVer") == "True")
				{
					if (Options.GetValue("FIPS186_3SigVer_mod1024") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA1") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA224") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA256") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA384") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA512") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA512224") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod1024SHA512256") == "True" && !TestPassed(results.SigVer931, "Mod 1024 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 1024 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVer_mod2048") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA1") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA224") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA256") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA384") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA512") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA512224") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod2048SHA512256") == "True" && !TestPassed(results.SigVer931, "Mod 2048 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 2048 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVer_mod3072") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA1") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA224") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA256") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA384") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA512") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA512224") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVer_mod3072SHA512256") == "True" && !TestPassed(results.SigVer931, "Mod 3072 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVer931 Mod 3072 SHA512256 failed");
					}
				}

				//SigVerPKCS15
				if (Options.GetValue("FIPS186_3SigVerPKCS15") == "True")
				{
					if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA1") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA384") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 1024 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 1024 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA1") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA384") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 2048 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 2048 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA1") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA384") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512224") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512256") == "True" && !TestPassed(results.SigVerPKCS15, "Mod 3072 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPKCS15 Mod 3072 SHA512256 failed");
					}
				}

				//SigVerPSS
				if (Options.GetValue("FIPS186_3SigVerPSS") == "True")
				{
					//Validate that the Salt Lengths provided are legal - they must be <= the digest size. SaltLen specified in bytes, so will check digest size / 8
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA1SaltLen")) > 20) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA1 salt length may not be greater than 20 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA256 salt length may not be greater than 32 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA384SaltLen")) > 48) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA384 salt length may not be greater than 48 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512SaltLen")) > 62) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512 salt length may not be greater than 62 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512256 salt length may not be greater than 32 bytes");

					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA1SaltLen")) > 20) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA1 salt length may not be greater than 20 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA256 salt length may not be greater than 32 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA384SaltLen")) > 48) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA384 salt length may not be greater than 48 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512SaltLen")) > 64) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512 salt length may not be greater than 64 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512256 salt length may not be greater than 32 bytes");

					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA1SaltLen")) > 20) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA1 salt length may not be greater than 20 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA256 salt length may not be greater than 32 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA384SaltLen")) > 48) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA384 salt length may not be greater than 48 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512SaltLen")) > 64) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512 salt length may not be greater than 64 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512256 salt length may not be greater than 32 bytes");

					if (Options.GetValue("FIPS186_3SigVerPSS_mod1024") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA1") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA224") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA256") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA384") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA512") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA512224") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod1024SHA512256") == "True" && !TestPassed(results.SigVerPSS, "Mod 1024 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 1024 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVerPSS_mod2048") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA1") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA224") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA256") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA384") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA512") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA512224") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod2048SHA512256") == "True" && !TestPassed(results.SigVerPSS, "Mod 2048 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 2048 SHA512256 failed");
					}

					if (Options.GetValue("FIPS186_3SigVerPSS_mod3072") == "True")
					{
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA1") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA  1 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA224") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA256") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA256 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA384") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA384 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA512") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA512224") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA512224:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512224 failed");
						if (Options.GetValue("FIPS186_3SigVerPSS_mod3072SHA512256") == "True" && !TestPassed(results.SigVerPSS, "Mod 3072 SHA512256:")) algoResult.AddFailure("FIPS 186-4 SigVerPSS Mod 3072 SHA512256 failed");
					}
				}
			}

			return algoResult;
		}
	}
}