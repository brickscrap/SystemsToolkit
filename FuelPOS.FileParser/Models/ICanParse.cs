namespace POSFileParser.Models
{
    public interface ICanParse
    {
        public string IDKey { get; set; }
        public void AddToItem(string[] headers, string value);
    }
}
