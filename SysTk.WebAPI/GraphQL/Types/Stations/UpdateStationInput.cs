namespace SysTk.WebAPI.GraphQL.Types.Stations
{
    public class UpdateStationInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Cluster { get; set; }
    }
}
