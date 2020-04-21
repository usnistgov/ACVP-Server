using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.RSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.RSA
{
	public class RSAKeyGen_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSAKeyGen_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSAKeyGen_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//ProbRP
				if (Options.GetValue("FIPS186_3KeyGen_ProbRP") == "True")
				{
					if (Options.GetValue("RSA2_ProbRP_Mod2048") == "True")
					{
						if (Options.GetValue("RSA2_ProbRP_TableC2") == "True" && !TestPassed(results.Results, "ProbRP Mod 2048  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbRP Mod 2048  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_ProbRP_TableC3") == "True" && !TestPassed(results.Results, "ProbRP Mod 2048  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbRP Mod 2048  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_ProbRP_Mod3072") == "True")
					{
						if (Options.GetValue("RSA2_ProbRP_TableC2") == "True" && !TestPassed(results.Results, "ProbRP Mod 3072  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbRP Mod 3072  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_ProbRP_TableC3") == "True" && !TestPassed(results.Results, "ProbRP Mod 3072  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbRP Mod 3072  M-RTable C.3 failed");
					}
				}

				//ProvRP
				if (Options.GetValue("FIPS186_3KeyGen_ProvRP") == "True")
				{
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA1") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA  1 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA224") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA224 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA256") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA256 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA384") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA384 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA512") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA512 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA512224") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA512224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA512224 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod2048SHA512256") == "True" && !TestPassed(results.Results, "ProvRP Mod 2048 SHA512256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 2048 SHA512256 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA1") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA  1 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA224") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA224 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA256") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA256 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA384") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA384 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA512") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA512 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA512224") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA512224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA512224 failed");
					if (Options.GetValue("RSA2_ProvRP_Mod3072SHA512256") == "True" && !TestPassed(results.Results, "ProvRP Mod 3072 SHA512256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvRP Mod 3072 SHA512256 failed");
				}

				//ProvPC
				if (Options.GetValue("FIPS186_3KeyGen_ProvPC") == "True")
				{
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA1") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA  1:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA  1 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA224") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA224 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA256") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA256 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA384") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA384:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA384 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA512") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA512:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA512 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA512224") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA512224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA512224 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod2048SHA512256") == "True" && !TestPassed(results.Results, "ProvPC Mod 2048 SHA512256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 2048 SHA512256 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA1") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA  1:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA  1 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA224") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA224 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA256") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA256 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA384") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA384:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA384 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA512") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA512:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA512 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA512224") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA512224:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA512224 failed");
					if (Options.GetValue("RSA2_ProvPC_Mod3072SHA512256") == "True" && !TestPassed(results.Results, "ProvPC Mod 3072 SHA512256:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProvPC Mod 3072 SHA512256 failed");
				}

				//BothPC
				if (Options.GetValue("FIPS186_3KeyGen_BothPC") == "True")
				{
					if (Options.GetValue("RSA2_BothPC_Mod2048SHA1") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA  1  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA  1  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA  1  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA  1  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA224") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA224  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA224  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA224  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA224  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA256") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA256  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA256  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA256  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA256  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA384") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA384  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA384  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA384  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA384  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA512") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA512224") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512224  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512224  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512224  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512224  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod2048SHA512256") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512256  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512256  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 2048 SHA512256  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 2048 SHA512256  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA1") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA  1  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA  1  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA  1  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA  1  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA224") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA224  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA224  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA224  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA224  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA256") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA256  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA256  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA256  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA256  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA384") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA384  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA384  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA384  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA384  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA512") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA512224") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512224  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512224  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512224  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512224  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_BothPC_Mod3072SHA512256") == "True")
					{
						if (Options.GetValue("RSA2_BothPC_TableC2") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512256  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512256  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_BothPC_TableC3") == "True" && !TestPassed(results.Results, "BothPC Mod 3072 SHA512256  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen BothPC Mod 3072 SHA512256  M-RTable C.3 failed");
					}
				}

				if (Options.GetValue("FIPS186_3KeyGen_ProbPC") == "True")
				{
					if (Options.GetValue("RSA2_ProbPC_Mod2048") == "True")
					{
						if (Options.GetValue("RSA2_ProbPC_TableC2") == "True" && !TestPassed(results.Results, "ProbPC Mod 2048  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbPC Mod 2048  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_ProbPC_TableC3") == "True" && !TestPassed(results.Results, "ProbPC Mod 2048  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbPC Mod 2048  M-RTable C.3 failed");
					}

					if (Options.GetValue("RSA2_ProbPC_Mod3072") == "True")
					{
						if (Options.GetValue("RSA2_ProbPC_TableC2") == "True" && !TestPassed(results.Results, "ProbPC Mod 3072  M-RTable C.2:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbPC Mod 3072  M-RTable C.2 failed");
						if (Options.GetValue("RSA2_ProbPC_TableC3") == "True" && !TestPassed(results.Results, "ProbPC Mod 3072  M-RTable C.3:")) algoResult.AddFailure("FIPS 186-4 KeyGen ProbPC Mod 3072  M-RTable C.3 failed");
					}
				}
			}

			return algoResult;
		}
	}
}