using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class SolutionDirectoryExtensions
{
    public static IEnumerable<SolutionReference> AggregateReferences(this SolutionDirectory solutionDirectory, Guid parentGuid)
    {
        foreach (var project in solutionDirectory.Projects ?? new List<string>())
        {
            var projectName = SupportedTypes.GetFileNameWithoutProjectExtension(project);
            var projectPath = ExpandByConvention(project);
            var projectGuid = project.DeriveGuid();
            yield return new SolutionReference(parentGuid, projectName, projectPath.AlignDirectorySeparators(), projectGuid);
        }

        foreach (var subDirectory in solutionDirectory.Directories ?? new Dictionary<string, SolutionDirectory>())
        {
            var subDirectoryGuid = $"{subDirectory.Key}{parentGuid}".DeriveGuid();
            yield return new SolutionReference(parentGuid, subDirectory.Key, subDirectory.Key, subDirectoryGuid);

            foreach (var projectReference in AggregateReferences(subDirectory.Value, subDirectoryGuid))
            {
                yield return projectReference;
            }
        }
    }

    private static string ExpandByConvention(string projectPath)
    {
        if (!SupportedTypes.HasProjectExtension(projectPath))
        {
            var fileName = Path.GetFileName(projectPath);
            return Path.Combine(projectPath.AlignDirectorySeparators(), $"{fileName}.csproj");
        }

        return projectPath;
    }
}