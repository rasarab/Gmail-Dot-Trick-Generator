using System;

namespace GoMan
{
    public class GmailDotGeneratorExceptionEmailUsernameTooLong : Exception
    {
        public GmailDotGeneratorExceptionEmailUsernameTooLong(string message) : base($"The username must be greater than less than or equal too 64 characters: {message}") { }
    }

    public class GmailDotGeneratorExceptionEmailUsernameTooShort : Exception
    {
        public GmailDotGeneratorExceptionEmailUsernameTooShort(string message) : base($"The username must be greater than 1 character: {message}") { }
    }

    public class GmailDotGeneratorExceptionEmailInvalid : Exception
    {
        public GmailDotGeneratorExceptionEmailInvalid(string message) : base($"The email entered is invalid: {message}") { }
    }

    public class GmailDotGeneratorExceptionNoAvaliableEmails : Exception
    {
        public GmailDotGeneratorExceptionNoAvaliableEmails(string message) : base($"No available emails: {message}") { }
    }

    public class GmailDotGeneratorExceptionEmailListIsEmptyOrNull : Exception
    {
        public GmailDotGeneratorExceptionEmailListIsEmptyOrNull(string message) : base($"Email list is empty or null: {message}"){}
    }
}
