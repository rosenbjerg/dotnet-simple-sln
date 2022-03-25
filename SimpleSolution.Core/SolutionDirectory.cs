using System.Collections.Generic;

namespace SimpleSolution.Core;

public class SolutionDirectory
{
    public Dictionary<string, SolutionDirectory> Directories { get; set; } = new();
    public List<string> Projects { get; set; } = new();
}