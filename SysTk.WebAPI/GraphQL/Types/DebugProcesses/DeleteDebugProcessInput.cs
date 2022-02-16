namespace SysTk.WebAPI.GraphQL.Types.DebugProcesses
{
    public class DeleteDebugProcessInput
    {
        [GraphQLType(typeof(NonNullType<IdType>))]
        public int Id { get; set; }
    }
}
