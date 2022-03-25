using System.Collections.Generic;

namespace SimpleSolution.Core;

public class SolutionRoot : SolutionDirectory
{
    public List<string> Configurations { get; set; } = new() { "Debug|Any CPU", "Release|Any CPU"};
}