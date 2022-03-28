using CommandLine;

namespace SimpleSolution.Cli;

[Verb("derive", HelpText = "Derive YAML specification from .sln file")]
class DeriveOptions
{
    [Option('i', "input-sln", Required = true, HelpText = "Path to the .NET solution file")]
    public string SolutionPath { get; set; } = null!;
}