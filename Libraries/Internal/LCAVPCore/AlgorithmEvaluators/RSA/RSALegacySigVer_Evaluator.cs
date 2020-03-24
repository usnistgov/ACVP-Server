using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.RSA;

namespace LCAVPCore.AlgorithmEvaluators.RSA
{
	public class RSALegacySigVer_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSALegacySigVer_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSALegacySigVer_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("Legacy_FIPS186_2SigVer") == "True")
				{
					if (Options.GetValue("Legacy_FIPS186_2SigVer_mod1024") == "True")
					{
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1024 SHA  1 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1024 SHA224 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1024 SHA256 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1024 SHA384 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1024 SHA512 failed");
					}

					if (Options.GetValue("Legacy_FIPS186_2SigVer_mod1536") == "True")
					{
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1536 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1536 SHA  1 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1536 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1536 SHA224 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1536 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1536 SHA256 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1536 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1536 SHA384 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 1536 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 1536 SHA512 failed");
					}

					if (Options.GetValue("Legacy_FIPS186_2SigVer_mod2048") == "True")
					{
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 2048 SHA  1 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 2048 SHA224 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 2048 SHA256 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 2048 SHA384 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 2048 SHA512 failed");
					}

					if (Options.GetValue("Legacy_FIPS186_2SigVer_mod3072") == "True")
					{
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 3072 SHA  1 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 3072 SHA224 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 3072 SHA256 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 3072 SHA384 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 3072 SHA512 failed");
					}

					if (Options.GetValue("Legacy_FIPS186_2SigVer_mod4096") == "True")
					{
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 4096 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 4096 SHA  1 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 4096 SHA224 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 4096 SHA256 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 4096 SHA384 failed");
						if (Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True" && !TestPassed(results.SigVer931_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVer Mod 4096 SHA512 failed");
					}
				}

				//186-2 SigVerPKCS15
				if (Options.GetValue("Legacy_PKCS#1_15SigVer") == "True")
				{
					if (Options.GetValue("Legacy_PKCS#1_15SigVer_mod1024") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1024 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1024 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1024 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1024 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1024 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_15SigVer_mod1536") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1536 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1536 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1536 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1536 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1536 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1536 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1536 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1536 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 1536 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 1536 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_15SigVer_mod2048") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 2048 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 2048 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 2048 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 2048 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 2048 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_15SigVer_mod3072") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 3072 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 3072 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 3072 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 3072 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 3072 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_15SigVer_mod4096") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 4096 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 4096 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 4096 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 4096 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 4096 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True" && !TestPassed(results.SigVerPKCS15_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPKCS15 Mod 4096 SHA512 failed");
					}
				}

				//186-2 SigVerPSS
				if (Options.GetValue("Legacy_PKCS#1_PSSSigVer") == "True")
				{
					if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod1024") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1024 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1024 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1024 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1024 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1024 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1024 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1024 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1024 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1024 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1024 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod1536") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1536 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1536 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1536 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1536 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1536 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1536 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1536 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1536 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 1536 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 1536 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod2048") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 2048 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 2048 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 2048 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 2048 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 2048 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod3072") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 3072 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 3072 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 3072 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 3072 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 3072 SHA512 failed");
					}

					if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod4096") == "True")
					{
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 4096 SHA  1:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 4096 SHA  1 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 4096 SHA224 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 4096 SHA256 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 4096 SHA384 failed");
						if (Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True" && !TestPassed(results.SigVerPSS_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigVerPSS Mod 4096 SHA512 failed");
					}
				}
			}

			return algoResult;
		}
	}
}