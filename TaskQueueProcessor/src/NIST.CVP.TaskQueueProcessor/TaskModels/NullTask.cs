namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class NullTask : ITask
    {
        public int DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;
        public bool IsSample { get; set; }
        
        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}