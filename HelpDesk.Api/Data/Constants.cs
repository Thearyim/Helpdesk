namespace HelpDesk.Api.Data
{
    public static class TicketStatus
    {
        public const string Active = nameof(Active);
        public const string Completed = nameof(Completed);
        public const string New = nameof(New);
        public const string Duplicate = nameof(Duplicate);
    }

    public static class UserRole
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
    }
}
