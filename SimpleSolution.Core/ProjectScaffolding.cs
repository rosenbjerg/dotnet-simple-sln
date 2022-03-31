using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class ProjectScaffolding
{
    private static readonly string ProjectContent = @"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

</Project>
".TrimNewlines();

    public static void Scaffold(string directory, IEnumerable<SolutionReference> references)
    {
        var projects = references.Where(r => !r.IsProjectDirectory());
        foreach (var p in projects)
        {
            var path = Path.Combine(directory, p.ProjectPath.AlignDirectorySeparators());
            if (File.Exists(path))
            {
                continue;
            }

            var pathDirectory = Path.GetDirectoryName(path);
            if (pathDirectory != directory)
            {
                Directory.CreateDirectory(pathDirectory!);
            }

            Console.WriteLine($"Project '{path}' has been scaffolded");
            File.WriteAllText(path, ProjectContent);
        }
    }
}