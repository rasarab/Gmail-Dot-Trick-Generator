using System;
using Newtonsoft.Json;

namespace GoMan
{
    public class GmailDotGeneratorConfiguration : IDisposable
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public int TotalCombinations { get; private set; }
        public int MaximumEmails { get; set; }

        internal readonly int UsernameLength;
        internal readonly int UsernameLengthMinusOne;

        [JsonConstructor]
        public GmailDotGeneratorConfiguration(string email, int maximumEmails = 0)
        {
            this.Email = email;
            this.Username = email.Split('@')[0];
            this.UsernameLength = this.Username.Length;
            this.UsernameLengthMinusOne = this.UsernameLength - 1;
            this.TotalCombinations = (int) Math.Pow(2, this.UsernameLengthMinusOne);
            this.MaximumEmails = maximumEmails;

            if (!IsValidEmail(email)) throw new GmailDotGeneratorExceptionEmailInvalid(this);
            if (UsernameLength <= 1) throw new GmailDotGeneratorExceptionEmailUsernameTooShort(this);
            if (UsernameLength > 64) throw new GmailDotGeneratorExceptionEmailUsernameTooLong(this);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && (email.ToLower().Contains("gmail.com") || email.ToLower().Contains("googlemail.com"));
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"Email: {Email}, Username: {Username}, TotalCombinations: {TotalCombinations}, MaximumEmails: {MaximumEmails}, UsernameLength: {UsernameLength}";
        }

        public void Dispose()
        {
            Email = null;
            Username = null;
        }
    }
}
