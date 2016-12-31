using System;

namespace GoMan
{
    [Serializable]
    public class GmailDotGeneratorExceptionEmailUsernameTooLong : Exception
    {
        public GmailDotGeneratorExceptionEmailUsernameTooLong(string message) : base($"The username must be greater than less than or equal too 64 characters: {message}") { }
    }
    [Serializable]
    public class GmailDotGeneratorExceptionEmailUsernameTooShort : Exception
    {
        public GmailDotGeneratorExceptionEmailUsernameTooShort(string message) : base($"The username must be greater than 1 character: {message}") { }
    }
    [Serializable]
    public class GmailDotGeneratorExceptionEmailInvalid : Exception
    {
        public GmailDotGeneratorExceptionEmailInvalid(string message) : base($"The email entered is invalid: {message}") { }
    }
    [Serializable]
    public class GmailDotGeneratorExceptionNoAvaliableEmails : Exception
    {
        public GmailDotGeneratorExceptionNoAvaliableEmails(string message) : base($"No available emails: {message}") { }
    }
    [Serializable]
    public class GmailDotGeneratorExceptionEmailListIsEmptyOrNull : Exception
    {
        public GmailDotGeneratorExceptionEmailListIsEmptyOrNull(string message) : base($"Email list is empty or null: {message}"){}
    }
}
