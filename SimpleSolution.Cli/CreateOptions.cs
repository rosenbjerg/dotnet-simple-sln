using CommandLine;

namespace SimpleSolution.Cli;

[Verb("scaffold", HelpText = "Create or update .sln file from YAML specification")]
class CreateOptions {
    [Option('i', "input-sln-spec", Required = true, HelpText = "Path to the .NET solution specification YAML file")]
    public string SolutionSpecPath { get; set; } = null!;
    
    [Option('p', "scaffold-projects", Required = false, HelpText = "Whether to scaffold missing project files")]
    public bool ScaffoldProjects { get; set; }
}