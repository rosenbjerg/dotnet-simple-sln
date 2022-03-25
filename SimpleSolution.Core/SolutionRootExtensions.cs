using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleSolution.Core;

public static class SolutionRootExtensions
{
    public static IEnumerable<SolutionReference> AggregateReferences(this SolutionDirectory solutionDirectory, Guid parentGuid)
    {
        foreach (var project in solutionDirectory.Projects)
        {
            var projectName = Path.GetFileNameWithoutExtension(project);
            var projectPath = ExpandByConvention(project, projectName);
            var projectGuid = project.DeriveGuid();
            yield return new SolutionReference(parentGuid, projectName, projectPath.AlignDirectorySeparators(), projectGuid);
        }

        foreach (var subDirectory in solutionDirectory.Directories)
        {
            var subDirectoryGuid = $"{subDirectory.Key}{parentGuid}".DeriveGuid();
            yield return new SolutionReference(parentGuid, subDirectory.Key, subDirectory.Key, subDirectoryGuid);

            foreach (var projectReference in AggregateReferences(subDirectory.Value, subDirectoryGuid))
            {
                yield return projectReference;
            }
        }
    }

    private static string ExpandByConvention(string projectPath, string projectName)
    {
        var extension = Path.GetExtension(projectPath).ToLowerInvariant();
        if (extension is not (".csproj" or ".fsproj"))
        {
            return Path.Combine(projectPath.AlignDirectorySeparators(), $"{projectName}.csproj");
        }

        return projectPath;
    }
}