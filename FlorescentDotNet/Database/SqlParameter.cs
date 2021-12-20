namespace FlorescentDotNet.Database
{
    public class SqlParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SqlParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}