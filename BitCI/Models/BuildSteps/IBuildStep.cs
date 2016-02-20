namespace BitCI.Models.BuildSteps
{
    public interface IBuildStep
    {
        int Id { get; set; }
        int BuildId { get; set; }
        Build Build { get; set; }
        StepStatus Status { get; set; }
        string Value { get; set; }
        void Execute();
    }
}
