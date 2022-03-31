namespace SimpleSolution.Core;

public static class SupportedTypes
{
    private const string SolutionDirectoryTypeId = "2150E333-8FDC-42A3-9474-1A3956D46DE8";

    private static readonly Dictionary<string, string> ProjectTypes = new()
    {
        {".csproj", "2150E333-8FDC-42A3-9474-1A3956D46DE8"},
        {".fsproj", "F2A71F9B-5D33-465A-A702-920D77279786"},
        {".vbproj", "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC"},
        {".vcxproj", "8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942"}
    };
    
    public static bool HasProjectExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return ProjectTypes.ContainsKey(extension);
    }

    public static string GetFileNameWithoutProjectExtension(string fileName)
    {
        if (HasProjectExtension(fileName))
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        return Path.GetFileName(fileName);
    }
    
    public static string GetProjectTypeId(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (ProjectTypes.TryGetValue(extension, out var projectTypeId))
        {
            return projectTypeId;
        }
        
        return SolutionDirectoryTypeId;
    }
}