namespace SysTk.WebAPI.GraphQL.Types
{
    public class UserRoleType : EnumType<UserRole>
    {
    }

    public enum UserRole
    {
        Admin,
        Supervisor,
        Member
    }
}
