namespace HelpDesk.Api.Data
{
    public static class ExtensionMethods
    {
        public static bool IsAdmin(this UserSession session)
        {
            return session.UserRole == UserRole.Admin;
        }

        public static bool CanModify(this UserSession session, Ticket ticket)
        {
            return session.IsAdmin()
                || session.UserId == ticket.CreatedByUserId
                || session.UserId == ticket.AssignedToUserId;
        }
    }
}
