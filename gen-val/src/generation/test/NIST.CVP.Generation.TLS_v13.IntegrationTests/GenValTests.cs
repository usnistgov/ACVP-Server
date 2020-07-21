using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.TLSv13.RFC8446;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS_v13.IntegrationTests
{
	[TestFixture, LongRunningIntegrationTest]
	public class GenValTests : GenValTestsSingleRunnerBase
	{
		public override string Algorithm => "TLS-v1.3";
		public override string Mode => "KDF";
		public override string Revision => "RFC8446";
		public override AlgoMode AlgoMode => AlgoMode.Tls_v1_3_v1_0;

		public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


		protected override void ModifyTestCaseToFail(dynamic testCase)
		{
			var rand = new Random800_90();

			if (testCase.exporterMasterSecret != null)
			{
				var bs = new BitString(testCase.exporterMasterSecret.ToString());
				bs = rand.GetDifferentBitStringOfSameSize(bs);

				testCase.exporterMasterSecret = bs.ToHex();
			}
		}

		protected override string GetTestFileFewTestCases(string folderName)
		{
			var p = new Parameters
			{
				Algorithm = Algorithm,
				Mode = Mode,
				Revision = Revision,
				HmacAlg = new []
				{
					HashFunctions.Sha2_d256,
					HashFunctions.Sha3_d256,
				},
				IsSample = true
			};

			return CreateRegistration(folderName, p);
		}

		protected override string GetTestFileLotsOfTestCases(string folderName)
		{
			var p = new Parameters
			{
				Algorithm = Algorithm,
				Mode = Mode,
				Revision = Revision,
				HmacAlg = EnumHelpers.GetEnumsWithoutDefault<HashFunctions>().ToArray(),
				IsSample = false
			};

			return CreateRegistration(folderName, p);
		}
	}
}