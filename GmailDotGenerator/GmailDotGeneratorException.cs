using System;

namespace GoMan
{
    public class GmailDotGeneratorException : Exception
    {
        public GmailDotGeneratorConfiguration Configuration { get; }

        public GmailDotGeneratorException(GmailDotGeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }
    }

    public class GmailDotGeneratorExceptionEmailUsernameTooLong : GmailDotGeneratorException
    {
        private const string Msg = "The username must be greater than less than or equal too 64 characters.";
        public override string Message => $"{Msg} {Configuration}";

        public GmailDotGeneratorExceptionEmailUsernameTooLong(GmailDotGeneratorConfiguration configuration) : base(configuration)
        {
        }
    }

    public class GmailDotGeneratorExceptionEmailUsernameTooShort : GmailDotGeneratorException
    {
        private const string Msg = "The username must be greater than 1 character.";
        public override string Message => $"{Msg} {Configuration}";

        public GmailDotGeneratorExceptionEmailUsernameTooShort(GmailDotGeneratorConfiguration configuration) : base(configuration)
        {
        }
    }

    public class GmailDotGeneratorExceptionEmailInvalid : GmailDotGeneratorException
    {
        private const string Msg = "The email entered is invalid.";
        public override string Message => $"{Msg} {Configuration}";

        public GmailDotGeneratorExceptionEmailInvalid(GmailDotGeneratorConfiguration configuration) : base(configuration)
        {
        }

    }

    public class GmailDotGeneratorExceptionNoAvaliableEmails : GmailDotGeneratorException
    {
        private const string Msg = "No available emails.";
        public override string Message =>  $"{Msg} {Configuration}";

        public GmailDotGeneratorExceptionNoAvaliableEmails(GmailDotGeneratorConfiguration configuration) : base(configuration)
        {
        }
    }

    public class GmailDotGeneratorExceptionEmailListIsEmptyOrNull : GmailDotGeneratorException
    {
        private const string Msg = "Email list is empty or null.";
        public override string Message => $"{Msg} {Configuration}";

        public GmailDotGeneratorExceptionEmailListIsEmptyOrNull(GmailDotGeneratorConfiguration configuration) : base(configuration)
        {
        }
    }
}
