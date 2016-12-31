using Newtonsoft.Json;

namespace GoMan
{
    public class GmailDotGeneratorEmailModel
    {
        public string Email { get; }
        public bool Used { get; set; } = false;

        public GmailDotGeneratorEmailModel(string email)
        {
            this.Email = email;
        }

        public override string ToString()
        {
            return this.Email;
        }
    }
}
