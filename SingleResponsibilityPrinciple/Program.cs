Names names = new Names();


Console.ReadKey();

class NamesValidator
{
    public bool IsValid(string name)
    {
        return name.Length >= 2 &&
               name.Length < 25 &&
               char.IsUpper(name[0]) &&
               name.All(char.IsLetter);
    }
}

class StringsTextualRepository
{
    private static readonly string Separator = Environment.NewLine;
    
    public List<string> Read(string filePath)  
    {
        string fileContents = File.ReadAllText(filePath);
        return fileContents.Split(Separator).ToList();
    }

    public void Write(string filePath, List<string> strings)
    {
        File.WriteAllText(filePath, string.Join(Separator, strings));
    }
}

class Names
{
    private List<string> _names = new List<string>();
    private readonly NamesValidator _namesValidator = new NamesValidator();

    public void AddName(string name)
    {
        if (_namesValidator.IsValid(name))
        {
            _names.Add(name);
        }
    }

    public void AddNames(List<string> stringsFromFile)
    {
        foreach (var name in stringsFromFile)
        {
            AddName(name);
        }
    }
}