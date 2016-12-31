namespace GoMan
{
    public class GmailDotGeneratorEmailModel
    {
        public GmailDotGeneratorEmailModel(string email)
        {
            Email = email;
        }

        public string Email { get; }
        public bool Used { get; set; } = false;

        public override string ToString()
        {
            return Email;
        }
    }
}