using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class Component_KAS_FFC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public Component_KAS_FFC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new Component_KAS_FFC_ResultsExtractor(SubmissionPath);
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
				//Schemes must be selected and ZZTestOnly

				//Hybrid1
				if (Options.GetValue("KASFFC_Hybrid1") == "True" && Options.GetValue("KASFFC_Hybrid1_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_Hybrid1_Initiator") == "True")
					{
						if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_INITIATOR failed functionality test");
						if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_Hybrid1_Responder") == "True")
					{
						if (!results.DHHybrid1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_RESPONDER failed functionality test");
						if (!results.DHHybrid1_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1_NOKC_RESPONDER failed validity test");
					}
				}

				//MQV2
				if (Options.GetValue("KASFFC_MQV2") == "True" && Options.GetValue("KASFFC_MQV2_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_MQV2_Initiator") == "True")
					{
						if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_INITIATOR failed functionality test");
						if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_MQV2_Responder") == "True")
					{
						if (!results.MQV2_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_RESPONDER failed functionality test");
						if (!results.MQV2_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV2_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV2_NOKC_RESPONDER failed validity test");
					}
				}

				//Hybrid1Flow
				if (Options.GetValue("KASFFC_Hybrid1Flow") == "True" && Options.GetValue("KASFFC_Hybrid1Flow_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_Hybrid1Flow_Initiator") == "True")
					{
						if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_INITIATOR failed functionality test");
						if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_Hybrid1Flow_Responder") == "True")
					{
						if (!results.DHHybrid1Flow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_RESPONDER failed functionality test");
						if (!results.DHHybrid1Flow_Validity.FirstOrDefault(r => r.TestName == "[FFC_HYBRID1FLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_HYBRID1FLOW_NOKC_RESPONDER failed validity test");
					}
				}

				//MQV1
				if (Options.GetValue("KASFFC_MQV1") == "True" && Options.GetValue("KASFFC_MQV1_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_MQV1_Initiator") == "True")
					{
						if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_INITIATOR failed functionality test");
						if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_MQV1_Responder") == "True")
					{
						if (!results.MQV1_Functionality.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_RESPONDER failed functionality test");
						if (!results.MQV1_Validity.FirstOrDefault(r => r.TestName == "[FFC_MQV1_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_MQV1_NOKC_RESPONDER failed validity test");
					}
				}

				//Static
				if (Options.GetValue("KASFFC_Static") == "True" && Options.GetValue("KASFFC_Static_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_Static_Initiator") == "True")
					{
						if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_INITIATOR failed functionality test");
						if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_Static_Responder") == "True")
					{
						if (!results.DHStatic_Functionality.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_RESPONDER failed functionality test");
						if (!results.DHStatic_Validity.FirstOrDefault(r => r.TestName == "[FFC_STATIC_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_STATIC_NOKC_RESPONDER failed validity test");
					}
				}

				//Ephem
				if (Options.GetValue("KASFFC_Ephem") == "True" && Options.GetValue("KASFFC_Ephem_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_Ephem_Initiator") == "True")
					{
						if (!results.DHEphem_Functionality.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_INITIATOR failed");
						if (!results.DHEphem_Validity.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_Ephem_Responder") == "True")
					{
						if (!results.DHEphem_Functionality.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_RESPONDER failed");
						if (!results.DHEphem_Validity.FirstOrDefault(r => r.TestName == "[FFC_EPHEM_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_EPHEM_NOKC_RESPONDER failed validity test");
					}
				}

				//OneFlow
				if (Options.GetValue("KASFFC_OneFlow") == "True" && Options.GetValue("KASFFC_OneFlow_ZZTestOnly") == "True")
				{
					if (Options.GetValue("KASFFC_OneFlow_Initiator") == "True")
					{
						if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_INITIATOR failed");
						if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_INITIATOR]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_INITIATOR failed validity test");
					}

					if (Options.GetValue("KASFFC_OneFlow_Responder") == "True")
					{
						if (!results.DHOneFlow_Functionality.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_RESPONDER failed");
						if (!results.DHOneFlow_Validity.FirstOrDefault(r => r.TestName == "[FFC_ONEFLOW_NOKC_RESPONDER]")?.Pass ?? false) algoResult.AddFailure("FFC_ONEFLOW_NOKC_RESPONDER failed validity test");
					}
				}

			}

			return algoResult;
		}
	}
}
