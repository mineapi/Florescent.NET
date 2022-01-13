namespace FlorescentDotNet.Util;

public class DotEnvReader
{
    //Simple API to read environment variables from .env files.

    private string DotEnvFilePath { get; set; }
    private readonly Dictionary<string, string> _properties;

    public DotEnvReader(string dotEnvFilePath)
    {
        DotEnvFilePath = dotEnvFilePath;
        _properties = new Dictionary<string, string>();
        
        Build();
    }

    private void Build()
    {
        string[] lines = File.ReadAllLines(DotEnvFilePath);

        foreach (string line in lines)
        {
            string[] splitProperty = line.Split("=");
            _properties.Add(splitProperty[0], splitProperty[1]);
        }
    }

    public string GetProperty(string property)
    {
        return _properties[property];
    }
}