using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.RSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.RSA
{
	public class RSASigGen186_2_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSASigGen186_2_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSASigGen186_2_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//186-2 SigGen931 - weird in that the results are actually under the 186-4 SigGen931 section
				if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA224") == "True" && !TestPassed(results.SigGen931_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigGen931 Mod 4096 SHA224 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA256") == "True" && !TestPassed(results.SigGen931_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigGen931 Mod 4096 SHA256 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA384") == "True" && !TestPassed(results.SigGen931_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigGen931 Mod 4096 SHA384 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA512") == "True" && !TestPassed(results.SigGen931_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigGen931 Mod 4096 SHA512 failed");
				}

				//186-2 SigGenPKCS15 - weird in that the results are actually under the 186-4 SigGenPKCS15 section
				if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA224") == "True" && !TestPassed(results.SigGenPKCS15_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigGenPKCS15 Mod 4096 SHA224 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA256") == "True" && !TestPassed(results.SigGenPKCS15_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigGenPKCS15 Mod 4096 SHA256 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA384") == "True" && !TestPassed(results.SigGenPKCS15_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigGenPKCS15 Mod 4096 SHA384 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA512") == "True" && !TestPassed(results.SigGenPKCS15_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigGenPKCS15 Mod 4096 SHA512 failed");
				}

				//186-2 SigGenPSS - weird in that the results are actually under the 186-4 SigGenPSS section
				if (Options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096") == "True")
				{
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA224SaltLen")) > 28) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA224 salt length may not be greater than 28 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA256SaltLen")) > 32) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA256 salt length may not be greater than 32 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA384SaltLen")) > 48) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA384 salt length may not be greater than 48 bytes");
					if (ParsingHelper.ParseValueToInteger(Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA512SaltLen")) > 64) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA512 salt length may not be greater than 64 bytes");

					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA224") == "True" && !TestPassed(results.SigGenPSS_186_2, "Mod 4096 SHA224:")) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA224 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA256") == "True" && !TestPassed(results.SigGenPSS_186_2, "Mod 4096 SHA256:")) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA256 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA384") == "True" && !TestPassed(results.SigGenPSS_186_2, "Mod 4096 SHA384:")) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA384 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA512") == "True" && !TestPassed(results.SigGenPSS_186_2, "Mod 4096 SHA512:")) algoResult.AddFailure("FIPS 186-2 SigGenPSS Mod 4096 SHA512 failed");
				}
			}

			return algoResult;
		}
	}
}