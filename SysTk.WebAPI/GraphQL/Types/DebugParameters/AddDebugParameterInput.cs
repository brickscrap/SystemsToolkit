using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.DebugParameters
{
    public class AddDebugParameterInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [GraphQLType(typeof(NonNullType<IdType>))]
        public int DebugProcessId { get; set; }
    }
}
