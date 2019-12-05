namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class NullTask : ITask
    {
        public long DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;
        
        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}