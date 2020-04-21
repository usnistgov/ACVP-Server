using System;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;

namespace Web.Admin.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestSessionsController : ControllerBase
	{
		private readonly ILogger<TestSessionsController> _logger;
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;

		public TestSessionsController(ILogger<TestSessionsController> logger, ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService)
		{
			_logger = logger;
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
		}

		[HttpPost]
		public ActionResult<PagedEnumerable<TestSessionLite>> GetTestSessions(TestSessionListParameters param)
		{
			if (param == null)
				return new BadRequestResult();

			return _testSessionService.Get(param);
		}

		[HttpGet("{testSessionId}")]
		public ActionResult<TestSession> GetTestSessionDetails(long testSessionId)
		{
			var result = _testSessionService.Get(testSessionId);
			
			if (result == null)
				return new NotFoundResult();
			
			for (int i = 0; i < result.VectorSets.Count; i++)
			{
				result.VectorSets[i] = _vectorSetService.GetVectorSet(result.VectorSets[i].Id);
			}

			return result;
		}

		[HttpPost("{testSessionId}")]
		public Result CancelTestSession(long testSessionId)
		{
			return _testSessionService.Cancel(testSessionId);
		}

		[HttpPost("vectorSet/{vectorSetId}")]
		public Result CancelVectorSet(long vectorSetId)
		{
			return _vectorSetService.Cancel(vectorSetId);
		}

		[HttpGet("vectorSet/{vectorSetId}")]
		public ActionResult<VectorSet> GetTestVectorSet(long vectorSetId)
		{
			var result = _vectorSetService.GetVectorSet(vectorSetId);

			if (result == null)
				return new NotFoundResult();

			return result;
		}

		[HttpGet("vectorSet/{vectorSetId}/json/{fileType}")]
		public IActionResult GetJsonFileForVectorSet(long vectorSetId, string fileType)
		{
			if (Enum.TryParse<VectorSetJsonFileTypes>(fileType, true, out var parsedFileType))
			{
				var result = _vectorSetService.GetVectorFileJson(vectorSetId, parsedFileType);

				if (result == null)
					return new NotFoundResult();

				if (!string.IsNullOrEmpty(result))
				{
					return Content(result, "application/json");
				}
			}
			else
			{
				_logger.LogWarning($"{nameof(fileType)} ({fileType}) could not be parsed into a type of {nameof(VectorSetJsonFileTypes)}");
			}

			return new BadRequestResult();
		}

		[HttpPost("vectorSet/{vectorSetId}/requeue/generation")]
		public Result RequeueGenerationTask(long vectorSetId)
		{
			return _taskQueueService.RequeueGenerationTask(vectorSetId);
		}
		
		[HttpPost("vectorSet/{vectorSetId}/requeue/validation")]
		public Result RequeueValidationTask(long vectorSetId)
		{
			return _taskQueueService.RequeueValidationTask(vectorSetId);
		}
	}
}