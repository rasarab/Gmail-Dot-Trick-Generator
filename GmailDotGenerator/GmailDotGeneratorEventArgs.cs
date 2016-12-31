namespace GoMan
{
    public class GmailDotGeneratorEventArgs
    {
        public long EstimatedCompletionTime { get; }
        public int CreatedCount { get; }
        public int RemainingCount { get; }
        public GmailDotGeneratorEmailModel[] LastEmailsGenerated { get; }

        public GmailDotGeneratorEventArgs(int createdCount, int remainingCount, long estimatedCompletionTime, params GmailDotGeneratorEmailModel[] lastEmailsGenerated)
        {
            LastEmailsGenerated = lastEmailsGenerated;
            CreatedCount = createdCount;
            RemainingCount = remainingCount;
            EstimatedCompletionTime = estimatedCompletionTime;
        }
    }
}
