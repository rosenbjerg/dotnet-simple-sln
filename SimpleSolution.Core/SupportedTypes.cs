namespace SimpleSolution.Core;

public static class SupportedTypes
{
    private const string SolutionDirectoryTypeId = "2150E333-8FDC-42A3-9474-1A3956D46DE8";
    private const string CSharpProjectTypeId = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
    private const string FSharpProjectTypeId = "F2A71F9B-5D33-465A-A702-920D77279786";
    private const string VbNetProjectTypeId = "F184B08F-C81C-45F6-A57F-5ABD9991F28F";

    private static readonly Dictionary<string, string> ProjectTypes = new()
    {
        {".csproj", CSharpProjectTypeId},
        {".fsproj", FSharpProjectTypeId},
        {".vbproj", VbNetProjectTypeId}
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