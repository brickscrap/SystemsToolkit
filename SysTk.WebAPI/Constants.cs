namespace SysTk.WebAPI
{
    public static class Policies
    {
        public const string CanAdd = "CanAdd";
        public const string CanDelete = "CanDelete";
        public const string CanModifyUsers = "CanModifyUsers";
        public const string IsVerified = "IsVerified";
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Supervisor = "Supervisor";
        public const string Member = "Member";
    }
}
