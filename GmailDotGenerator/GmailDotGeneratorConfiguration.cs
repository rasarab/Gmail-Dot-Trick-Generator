using System;
using System.Net.Mail;
using Newtonsoft.Json;

namespace GoMan
{
    public sealed class GmailDotGeneratorConfiguration
    {
        internal readonly int UsernameLength;
        internal readonly int UsernameLengthMinusOne;

        [JsonConstructor]
        public GmailDotGeneratorConfiguration(string email, int maximumEmails = 0)
        {
            Email = email;
            Username = email.Split('@')[0];
            UsernameLength = Username.Length;
            UsernameLengthMinusOne = UsernameLength - 1;
            TotalCombinations = (int) Math.Pow(2, UsernameLengthMinusOne);
            MaximumEmails = maximumEmails;

            if (!IsValidEmail(email)) throw new GmailDotGeneratorExceptionEmailInvalid(ToString());
            if (UsernameLength <= 1) throw new GmailDotGeneratorExceptionEmailUsernameTooShort(ToString());
            if (UsernameLength > 64) throw new GmailDotGeneratorExceptionEmailUsernameTooLong(ToString());
        }

        public string Email { get; private set; }
        public string Username { get; private set; }
        public int TotalCombinations { get; }
        public int MaximumEmails { get; set; }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email &&
                       (email.ToLower().Contains("gmail.com") || email.ToLower().Contains("googlemail.com"));
            }
            catch
            {
                return false;
            }
        }

        public sealed override string ToString()
        {
            return
                $"Email: {Email}, Username: {Username}, TotalCombinations: {TotalCombinations}, MaximumEmails: {MaximumEmails}, UsernameLength: {UsernameLength}";
        }
    }
}