namespace SimpleSolution.Core.Models;

public class SolutionRootDirectory : SolutionDirectory
{
    public List<string> Configurations { get; set; } = new() { "Debug|Any CPU", "Release|Any CPU"};
}