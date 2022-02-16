namespace SysTk.WebAPI.GraphQL.Types.DebugParameters
{
    public class UpdateDebugParameterInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DebugProcessName { get; set; }
    }
}
