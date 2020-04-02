using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly ILogger<WorkflowsController> _logger;
        private readonly IWorkflowService _workflowService;

        public WorkflowsController(ILogger<WorkflowsController> logger, IWorkflowService workflowService)
        {
            _logger = logger;
            _workflowService = workflowService;
        }
        
        [HttpPost]
        public ActionResult<PagedEnumerable<WorkflowItemLite>> GetWorkflows(WorkflowListParameters param)
        {
            if (param == null)
                return new BadRequestResult();

            return _workflowService.GetWorkflowItems(param);
        }

        [HttpGet("{workflowId}")]
        public ActionResult<WorkflowItem> GetWorkFlow(long workflowId)
        {
            var result = _workflowService.GetWorkflowItem(workflowId);
            
            if (result == null)
                return new NotFoundResult();

            return result;
        }

        [HttpPost("{workflowId}/approve")]
        public ActionResult<Result> ApproveWorkflow(long workflowId)
        {
            var workflow = _workflowService.GetWorkflowItem(workflowId);

            if (workflow == null)
            {
                return new NotFoundResult();
            }
            
            return _workflowService.Approve(workflow);
        }
        
        [HttpPost("{workflowId}/reject")]
        public ActionResult<Result> RejectWorkflow(long workflowId)
        {
            var workflow = _workflowService.GetWorkflowItem(workflowId);

            if (workflow == null)
            {
                return new NotFoundResult();
            }
            
            return _workflowService.Reject(workflow);
        }
    }
}