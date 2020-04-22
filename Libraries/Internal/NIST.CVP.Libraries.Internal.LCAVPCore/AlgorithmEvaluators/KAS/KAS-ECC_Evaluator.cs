using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KAS;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.KAS
{
	public class KAS_ECC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public KAS_ECC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new KAS_ECC_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			//Go through the algo's options, figure out what needs to be tested, and check for it
			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{

				//Schemes must be selected and NOT ZZTestOnly

				//FullUnified
				if (Options.GetValue("KASECC_FullUnified") == "True" && Options.GetValue("KASECC_FullUnified_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_FullUnified_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_FullUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_FullUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_FullUnified_Unilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullUnified_Bilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_FullUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_FullUnified_Unilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullUnified_Bilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_NOKC_INITIATOR failed functionality test");
							if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_FullUnified_Responder") == "True")
					{
						if (Options.GetValue("KASECC_FullUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_FullUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_FullUnified_Unilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullUnified_Bilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_FullUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_FullUnified_Unilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullUnified_Bilateral") == "True")
								{
									if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.FullUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_NOKC_RESPONDER failed functionality test");
							if (!results.FullUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLUNIFIED_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//FullMQV
				if (Options.GetValue("KASECC_FullMQV") == "True" && Options.GetValue("KASECC_FullMQV_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_FullMQV_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_FullMQV_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_FullMQV_Provider") == "True")
							{
								if (Options.GetValue("KASECC_FullMQV_Unilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullMQV_Bilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_FullMQV_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_FullMQV_Unilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullMQV_Bilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_NOKC_INITIATOR failed functionality test");
							if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_FullMQV_Responder") == "True")
					{
						if (Options.GetValue("KASECC_FullMQV_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_FullMQV_Provider") == "True")
							{
								if (Options.GetValue("KASECC_FullMQV_Unilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullMQV_Bilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_FullMQV_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_FullMQV_Unilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_FullMQV_Bilateral") == "True")
								{
									if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.FullMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_NOKC_RESPONDER failed functionality test");
							if (!results.FullMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_FULLMQV_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_FULLMQV_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//OnePassUnified
				if (Options.GetValue("KASECC_OnePassUnified") == "True" && Options.GetValue("KASECC_OnePassUnified_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_OnePassUnified_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_OnePassUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_OnePassUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_OnePassUnified_Unilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassUnified_Bilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_OnePassUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_OnePassUnified_Unilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassUnified_Bilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_NOKC_INITIATOR failed functionality test");
							if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_OnePassUnified_Responder") == "True")
					{
						if (Options.GetValue("KASECC_OnePassUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_OnePassUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_OnePassUnified_Unilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassUnified_Bilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_OnePassUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_OnePassUnified_Unilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassUnified_Bilateral") == "True")
								{
									if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.OnePassUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_NOKC_RESPONDER failed functionality test");
							if (!results.OnePassUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSUNIFIED_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//OnePassMQV
				if (Options.GetValue("KASECC_OnePassMQV") == "True" && Options.GetValue("KASECC_OnePassMQV_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_OnePassMQV_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_OnePassMQV_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_OnePassMQV_Provider") == "True")
							{
								if (Options.GetValue("KASECC_OnePassMQV_Unilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassMQV_Bilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_OnePassMQV_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_OnePassMQV_Unilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassMQV_Bilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_NOKC_INITIATOR failed functionality test");
							if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_OnePassMQV_Responder") == "True")
					{
						if (Options.GetValue("KASECC_OnePassMQV_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_OnePassMQV_Provider") == "True")
							{
								if (Options.GetValue("KASECC_OnePassMQV_Unilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassMQV_Bilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_OnePassMQV_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_OnePassMQV_Unilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_OnePassMQV_Bilateral") == "True")
								{
									if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.OnePassMQV_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_NOKC_RESPONDER failed functionality test");
							if (!results.OnePassMQV_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSMQV_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSMQV_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//StaticUnified
				if (Options.GetValue("KASECC_StaticUnified") == "True" && Options.GetValue("KASECC_StaticUnified_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_StaticUnified_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_StaticUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_StaticUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_StaticUnified_Unilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_StaticUnified_Bilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_StaticUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_StaticUnified_Unilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_StaticUnified_Bilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_NOKC_INITIATOR failed functionality test");
							if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_StaticUnified_Responder") == "True")
					{
						if (Options.GetValue("KASECC_StaticUnified_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASECC_StaticUnified_Provider") == "True")
							{
								if (Options.GetValue("KASECC_StaticUnified_Unilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_StaticUnified_Bilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASECC_StaticUnified_Recipient") == "True")
							{
								if (Options.GetValue("KASECC_StaticUnified_Unilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASECC_StaticUnified_Bilateral") == "True")
								{
									if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.StaticUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_NOKC_RESPONDER failed functionality test");
							if (!results.StaticUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_STATICUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_STATICUNIFIED_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//Ephem
				if (Options.GetValue("KASECC_EphemUnified") == "True" && Options.GetValue("KASECC_EphemUnified_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_EphemUnified_Initiator") == "True")
					{
						if (!results.EphemeralUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_EPHEMUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_EPHEMUNIFIED_NOKC_INITIATOR failed functionality test");
						if (!results.EphemeralUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_EPHEMUNIFIED_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_EPHEMUNIFIED_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASECC_EphemUnified_Responder") == "True")
					{
						if (!results.EphemeralUnified_Functionality.FirstOrDefault(r => r.TestName == "[ECC_EPHEMUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_EPHEMUNIFIED_NOKC_RESPONDER failed functionality test");
						if (!results.EphemeralUnified_Validity.FirstOrDefault(r => r.TestName == "[ECC_EPHEMUNIFIED_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_EPHEMUNIFIED_NOKC_RESPONDER failed validity test");
					}
				}

				//OnePassDH
				if (Options.GetValue("KASECC_OnePassDH") == "True" && Options.GetValue("KASECC_OnePassDH_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASECC_OnePassDH_Initiator") == "True")
					{
						if (Options.GetValue("KASECC_OnePassDH_KeyConfirm") == "True")
						{
							if (!results.OnePassDH_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
							if (!results.OnePassDH_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
						}
						else
						{
							if (!results.OnePassDH_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_NOKC_INITIATOR failed functionality test");
							if (!results.OnePassDH_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASECC_OnePassDH_Responder") == "True")
					{
						if (Options.GetValue("KASECC_OnePassDH_KeyConfirm") == "True")
						{
							if (!results.OnePassDH_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
							if (!results.OnePassDH_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
						}
						else
						{
							if (!results.OnePassDH_Functionality.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_NOKC_RESPONDER failed functionality test");
							if (!results.OnePassDH_Validity.FirstOrDefault(r => r.TestName == "[ECC_ONEPASSDH_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("ECC_ONEPASSDH_NOKC_RESPONDER failed validity test");
						}
					}
				}
			}

			return algoResult;
		}
	}
}
