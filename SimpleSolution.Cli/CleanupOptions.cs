using CommandLine;

namespace SimpleSolution.Cli;

[Verb("cleanup", HelpText = "Cleanup .sln file")]
class CleanupOptions
{
    [Option('i', "input-sln", Required = true, HelpText = "Path to the .NET solution file")]
    public string SolutionPath { get; set; } = null!;
    
    [Option('s', "create-spec", Required = false, HelpText = "Whether to output the solution specification to a YAML file")]
    public bool CreateSpecificationFile { get; set; }
}