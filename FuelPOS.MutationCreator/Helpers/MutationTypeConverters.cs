namespace FuelPOS.MutationCreator.Helpers
{
    public static class MutationTypeConverters
    {
        public static string ToMutation(this bool input)
        {
            return (input ? "YES" : "NO");
        }
    }
}
