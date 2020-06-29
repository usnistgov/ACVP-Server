using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IVectorSetProvider
	{
		Result Cancel(long id);
		//Result UpdateSubmittedResults(long vectorSetID, string results);
		Result UpdateStatus(long vectorSetID, VectorSetStatus status, string errorMessage);
		Result Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID);
		List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetIDsForTestSession(long testSessionID);
		VectorSet GetVectorSet(long vectorSetId);
		List<VectorSetJsonFile> GetVectorSetJsonFilesAvailable(long vectorSetId);
		string GetVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType);
		List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)> GetVectorFileJson(long vectorSetID);
		Result InsertVectorSetJson(long vectorSetID, VectorSetJsonFileTypes fileType, string json);
		Result Archive(long vectorSetId);
		List<long> GetVectorSetsToArchive();
		Result RemoveVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType);
	}
}