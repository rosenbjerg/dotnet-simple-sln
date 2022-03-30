using System.Text.RegularExpressions;
using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class SpecificationDeriver
{
    public static SolutionRootDirectory DeriveFromSolution(string solutionPath, bool keepMissingProjects)
    {
        var solutionId = Path.GetFileNameWithoutExtension(solutionPath).DeriveGuid();
        var slnContent = File.ReadAllText(solutionPath);
        var nestingSectionRegex = new Regex("GlobalSection\\(NestedProjects\\) = preSolution\\s(\\s+{[^}]+} = {[^}]+})+\\s+EndGlobalSection",
            RegexOptions.Compiled);
        var nestingRegex = new Regex("{([^}]+)} = {([^}]+)}", RegexOptions.Compiled);
        var nestings = nestingSectionRegex.Matches(slnContent).SelectMany(m => m.Groups[1].Captures).Select(c =>
        {
            var nesting = nestingRegex.Match(c.Value);
            return (Guid.Parse(nesting.Groups[1].Value), Guid.Parse(nesting.Groups[2].Value));
        }).ToDictionary(n => n.Item1, n => n.Item2);


        var solutionDirectory = Path.GetDirectoryName(solutionPath);
        var projectRegex = new Regex("Project\\(\"{([^}]+)}\"\\) = \"([^\"]+)\", \"([^\"]+)\", \"{([^}]+)}\"\\s*EndProject", RegexOptions.Compiled);
        var references = projectRegex.Matches(slnContent).Select(match =>
        {
            var projectId = Guid.Parse(match.Groups[4].Value);
            if (!nestings.TryGetValue(projectId, out var parentId))
            {
                parentId = solutionId;
            }

            return new SolutionReference(parentId, match.Groups[2].Value, match.Groups[3].Value.AlignDirectorySeparators(), projectId);
        })
            .Where(r => keepMissingProjects || r.ProjectName == r.ProjectPath || File.Exists(Path.Combine(solutionDirectory!, r.ProjectPath)))
            .OrderBy(r => r.ProjectPath)
            .GroupBy(r => r.ParentId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var solutionRootDirectory = new SolutionRootDirectory();
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