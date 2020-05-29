using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskQueueController : ControllerBase
	{
		private readonly ITaskQueueService _taskQueueService;

		public TaskQueueController(ITaskQueueService taskQueueService)
		{
			_taskQueueService = taskQueueService;
		}

		[HttpGet]
		public ActionResult<WrappedEnumerable<TaskQueueItem>> GetTaskQueue() => _taskQueueService.List().ToWrappedEnumerable();

		[HttpDelete("{taskID}")]
		public Result DeleteTask(long taskID) => _taskQueueService.Delete(taskID);

		//Should be a RestartAll endpoint here too, wasn't sure what to make it
	}
}