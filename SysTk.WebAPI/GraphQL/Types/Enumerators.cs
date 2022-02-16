using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class UserRoleType : EnumType<UserRole>
    {
    }

    public class ClusterType : EnumType<Cluster> { }

    public enum UserRole
    {
        Admin,
        Supervisor,
        Member
    }
}
