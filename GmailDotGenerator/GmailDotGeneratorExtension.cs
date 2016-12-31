namespace GoMan
{
    public static class GmailDotGeneratorExtension
    {
        public static string UseEmail(this GmailDotGeneratorEmailModel gmailDotEmailModel)
        {
            gmailDotEmailModel.Used = true;
            return gmailDotEmailModel.Email;
        }
    }
}
