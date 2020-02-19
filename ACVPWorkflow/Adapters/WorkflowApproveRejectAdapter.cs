using ACVPWorkflow.Results;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.Adapters
{
    public class WorkflowApproveRejectAdapter : IWorkflowApproveRejectAdapter
    {
        private readonly IWorkflowService _workflowService;
        private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;

        public WorkflowApproveRejectAdapter(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory)
        {
            _workflowService = workflowService;
            _workflowItemProcessorFactory = workflowItemProcessorFactory;
        }

        public KillThisResult Approve(long workflowId)
        {
            var workflowItem = _workflowService.GetWorkflowItem(workflowId);
            
            if (workflowItem == null)
                return new KillThisResult($"Could not find workflow item to approve. {nameof(workflowId)}: {workflowId}");

            var workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(workflowItem.APIAction);
            workflowItemProcessor.Approve(workflowItem);
            
            return new KillThisResult();
        }

        public KillThisResult Reject(long workflowId)
        {
            var workflowItem = _workflowService.GetWorkflowItem(workflowId);
            
            if (workflowItem == null)
                return new KillThisResult($"Could not find workflow item to reject. {nameof(workflowId)}: {workflowId}");

            var workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(workflowItem.APIAction);
            workflowItemProcessor.Reject(workflowItem);
            
            return new KillThisResult();
        }
    }
}