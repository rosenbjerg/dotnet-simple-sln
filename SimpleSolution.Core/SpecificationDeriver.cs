using System.Text.RegularExpressions;
using Microsoft.Build.Construction;
using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class SpecificationDeriver
{
    public static SolutionRootDirectory DeriveFromSolution(string solutionPath, bool keepMissingProjects)
    {
        var sln = SolutionFile.Parse(Path.GetFullPath(solutionPath));
        var solutionDirectory = Path.GetDirectoryName(solutionPath);
        var solutionId = Path.GetFileName(solutionPath).DeriveGuid();
        var idMap = sln.ProjectsInOrder.ToDictionary(p => p.ProjectGuid, p => p.RelativePath.DeriveGuid());
        idMap[solutionId.ToString()] = solutionId;
        var references = sln.ProjectsInOrder
            .Select(p => new SolutionReference(idMap[p.ParentProjectGuid ?? solutionId.ToString()], p.ProjectName, p.RelativePath, p.RelativePath.DeriveGuid()))
            .Where(r => keepMissingProjects || r.ProjectName == r.ProjectPath || File.Exists(Path.Combine(solutionDirectory!, r.ProjectPath)))
            .GroupBy(r => r.ParentId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var solutionRootDirectory = new SolutionRootDirectory
        {
            Configurations = sln.SolutionConfigurations.Select(c => c.FullName).ToList()
        };
        Populate(solutionRootDirectory, references, solutionId);

        return solutionRootDirectory;
    }

    private static void Populate(SolutionDirectory directory, IReadOnlyDictionary<Guid, SolutionReference[]> references, Guid parentId)
    {
        var subDirectories = references[parentId].Where(r => r.ProjectName == r.ProjectPath).ToArray();
        foreach (var solutionDirectory in subDirectories)
        {
            var subDirectory = new SolutionDirectory();
            directory.Directories.Add(solutionDirectory.ProjectName, subDirectory);
            Populate(subDirectory, references, solutionDirectory.ProjectId);
        }

        var projects = references[parentId]
            .Where(r => r.ProjectName != r.ProjectPath)
            .Select(r =>
            {
                var full = $"{r.ProjectName}{Path.DirectorySeparatorChar}{r.ProjectName}.csproj";
                return r.ProjectPath.Replace(full, r.ProjectName);
            }).ToArray();

        if (projects.Any())
        {
            directory.Projects.AddRange(projects);
        }
    }
}