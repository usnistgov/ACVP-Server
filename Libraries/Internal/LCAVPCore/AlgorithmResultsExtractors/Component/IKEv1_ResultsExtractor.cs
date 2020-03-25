using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.Component;

namespace LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class IKEv1_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "kdf135_Summary.txt";
		private string _summaryFilePath;

		public IKEv1_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public IKEv1_Results Extract()
		{
			IKEv1_Results results = new IKEv1_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CBC sections
			string line;
			int i;

			for (i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that isn't the IKEv1 line
				if (!line.StartsWith("IKE v1 KDF")) continue;

				//Found the IKEv1 line, so the next 3 should be lines we care about. First, make sure there are 3 lines left, even if they're the wrong lines (this is really just a basic bounds check to prevent overflow)
				if (lines.Length < i + 3)
				{
					results.InvalidReasons.Add("Expected IKEv1 results lines not found");
					break;
				}


				string[] parts;

				//First of the 3 lines is Digital Signature Authentication
				line = lines[i + 1];
				parts = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2 && parts[0].Trim() == "Digital Signature Authentication:") results.DigitalSignatureAuthenticationPassed = parts[1] == "PASSED";
				else results.InvalidReasons.Add("Digital Signature Authentication line not found");

				//Second line is Public Key Encryption Authentication
				line = lines[i + 2];
				parts = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2 && parts[0].Trim() == "Public Key Encryption Authentication:") results.PublicKeyEncryptionAuthenticationPassed = parts[1] == "PASSED";
				else results.InvalidReasons.Add("Public Key Encryption Authentication line not found");

				//Third line is Preshared Key Authentication
				line = lines[i + 3];
				parts = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2 && parts[0].Trim() == "Pre-shared Key Authentication:") results.PresharedKeyAuthenticationPassed = parts[1] == "PASSED";
				else results.InvalidReasons.Add("Pre-shared Key Authentication line not found");

				break;
			}

			//Handle error case of IKEv1 line not being found
			if (i == lines.Length) results.InvalidReasons.Add("IKEv1 result not found");

			return results;
		}
	}
}
