using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.TDES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.TDES
{
	public class TDES_KW_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "KeyWrap38F_Summary.txt";
		private string _summaryFilePath;

		public TDES_KW_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TDES_KW_Results Extract()
		{
			TDES_KW_Results results = new TDES_KW_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content - since we want only the lines starting with KW-AE or KW-AD, filter to that to get rid of the KWP and TDES-KW lines
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => l.StartsWith("TKW-A")).ToArray();

			results.EncryptPassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "TKW-AE with TDEA cipher function") == "PASSED";
			results.EncryptInversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "TKW-AE with TDEA inverse cipher function") == "PASSED";
			results.DecryptPassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "TKW-AD with TDEA cipher function") == "PASSED";
			results.DecryptInversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "TKW-AD with TDEA inverse cipher function") == "PASSED";

			return results;
		}
	}
}