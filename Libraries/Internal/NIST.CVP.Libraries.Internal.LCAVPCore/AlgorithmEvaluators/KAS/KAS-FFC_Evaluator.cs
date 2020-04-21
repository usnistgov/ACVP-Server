using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KAS;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.KAS
{
	public class KAS_FFC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public KAS_FFC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new KAS_FFC_ResultsExtractor(SubmissionPath);
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

				//Hybrid1
				if (Options.GetValue("KASFFC_Hybrid1") == "True" && Options.GetValue("KASFFC_Hybrid1_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_Hybrid1_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_Hybrid1_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Hybrid1_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1_Unilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1_Bilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Hybrid1_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1_Unilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1_Bilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_INITIATOR failed functionality test");
							if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_Hybrid1_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_Hybrid1_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Hybrid1_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1_Unilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1_Bilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Hybrid1_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1_Unilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1_Bilateral") == "True")
								{
									if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_RESPONDER failed functionality test");
							if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//MQV2
				if (Options.GetValue("KASFFC_MQV2") == "True" && Options.GetValue("KASFFC_MQV2_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_MQV2_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_MQV2_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_MQV2_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_MQV2_Unilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV2_Bilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_MQV2_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_MQV2_Unilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV2_Bilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_INITIATOR failed functionality test");
							if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_MQV2_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_MQV2_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_MQV2_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_MQV2_Unilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV2_Bilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_MQV2_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_MQV2_Unilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV2_Bilateral") == "True")
								{
									if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_RESPONDER failed functionality test");
							if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//Hybrid1Flow
				if (Options.GetValue("KASFFC_Hybrid1Flow") == "True" && Options.GetValue("KASFFC_Hybrid1Flow_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_Hybrid1Flow_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_Hybrid1Flow_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Hybrid1Flow_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1Flow_Unilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1Flow_Bilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Hybrid1Flow_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1Flow_Unilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1Flow_Bilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_INITIATOR failed functionality test");
							if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_Hybrid1Flow_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_Hybrid1Flow_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Hybrid1Flow_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1Flow_Unilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1Flow_Bilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Hybrid1Flow_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Hybrid1Flow_Unilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Hybrid1Flow_Bilateral") == "True")
								{
									if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_RESPONDER failed functionality test");
							if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//MQV1
				if (Options.GetValue("KASFFC_MQV1") == "True" && Options.GetValue("KASFFC_MQV1_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_MQV1_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_MQV1_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_MQV1_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_MQV1_Unilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV1_Bilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_MQV1_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_MQV1_Unilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV1_Bilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_INITIATOR failed functionality test");
							if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_MQV1_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_MQV1_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_MQV1_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_MQV1_Unilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV1_Bilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_MQV1_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_MQV1_Unilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_MQV1_Bilateral") == "True")
								{
									if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_RESPONDER failed functionality test");
							if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//Static
				if (Options.GetValue("KASFFC_Static") == "True" && Options.GetValue("KASFFC_Static_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_Static_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_Static_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Static_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Static_Unilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Static_Bilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Static_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Static_Unilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Static_Bilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_INITIATOR_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_INITIATOR_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_INITIATOR failed functionality test");
							if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_Static_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_Static_KeyConfirm") == "True")
						{
							if (Options.GetValue("KASFFC_Static_Provider") == "True")
							{
								if (Options.GetValue("KASFFC_Static_Unilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Static_Bilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_PROVIDER_BILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_PROVIDER_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_PROVIDER_BILATERAL failed validity test");
								}
							}

							if (Options.GetValue("KASFFC_Static_Recipient") == "True")
							{
								if (Options.GetValue("KASFFC_Static_Unilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_RECIPIENT_UNILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_RECIPIENT_UNILATERAL failed validity test");
								}

								if (Options.GetValue("KASFFC_Static_Bilateral") == "True")
								{
									if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_RECIPIENT_BILATERAL failed functionality test");
									if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_KC_RESPONDER_RECIPIENT_BILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_KC_RESPONDER_RECIPIENT_BILATERAL failed validity test");
								}
							}
						}
						else
						{
							if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_RESPONDER failed functionality test");
							if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_RESPONDER failed validity test");
						}
					}
				}

				//Ephem
				if (Options.GetValue("KASFFC_Ephem") == "True" && Options.GetValue("KASFFC_Ephem_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_Ephem_Initiator") == "True")
					{
						if (!results.DHEphem_Functionality.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_INITIATOR failed functionality test");
						if (!results.DHEphem_Validity.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_Ephem_Responder") == "True")
					{
						if (!results.DHEphem_Functionality.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_RESPONDER failed functionality test");
						if (!results.DHEphem_Validity.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_RESPONDER failed validity test");
					}
				}

				//OneFlow
				if (Options.GetValue("KASFFC_OneFlow") == "True" && Options.GetValue("KASFFC_OneFlow_ZZTestOnly") == "False")
				{
					if (Options.GetValue("KASFFC_OneFlow_Initiator") == "True")
					{
						if (Options.GetValue("KASFFC_OneFlow_KeyConfirm") == "True")
						{
							if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_KC_INITIATOR_RECIPIENT_UNILATERAL failed functionality test");
							if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_KC_INITIATOR_RECIPIENT_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_KC_INITIATOR_RECIPIENT_UNILATERAL failed validity test");
						}
						else
						{
							if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_INITIATOR failed functionality test");
							if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_INITIATOR failed validity test");
						}
					}

					if (Options.GetValue("KASFFC_OneFlow_Responder") == "True")
					{
						if (Options.GetValue("KASFFC_OneFlow_KeyConfirm") == "True")
						{
							if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_KC_RESPONDER_PROVIDER_UNILATERAL failed functionality test");
							if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_KC_RESPONDER_PROVIDER_UNILATERAL]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_KC_RESPONDER_PROVIDER_UNILATERAL failed validity test");
						}
						else
						{
							if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_RESPONDER failed functionality test");
							if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_RESPONDER failed validity test");
						}
					}
				}
			}

			return algoResult;
		}
	}
}
