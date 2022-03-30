namespace SimpleSolution.Core.Models;

public class SolutionDirectory
{
    public List<string> Projects { get; set; } = new();
    public Dictionary<string, SolutionDirectory> Directories { get; set; } = new();
}