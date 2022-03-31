using CommandLine;
using SimpleSolution.Core;
using SimpleSolution.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SimpleSolution.Cli;

internal static class Program
{
    private static int Main(string[] args)
    {
        var started = DateTime.UtcNow;
        var result = Parser.Default.ParseArguments<CreateOptions, DeriveOptions, CleanupOptions>(args)
            .MapResult(
                (CreateOptions options) => RunCreateAndReturnExitCode(options),
                (DeriveOptions options) => RunDeriveAndReturnExitCode(options),
                (CleanupOptions options) => RunCleanupAndReturnExitCode(options),
                errors => 1);
        
        var elapsed = DateTime.UtcNow.Subtract(started).TotalMilliseconds;
        Console.WriteLine($"Completed in {elapsed}ms");
        return result;
    }

    private static int RunCreateAndReturnExitCode(CreateOptions options)
    {
        var directory = Path.GetDirectoryName(options.SolutionSpecPath) ?? ".";
        var solutionName = Path.GetFileNameWithoutExtension(options.SolutionSpecPath).Replace(".sln", "");

        var solutionRoot = ParseYamlSolutionSpecification(options);

        var solutionId = Path.GetFileName(options.SolutionSpecPath).DeriveGuid();
        var references = solutionRoot.AggregateReferences(solutionId).ToArray();
        WriteSolutionToFile(solutionId, references, solutionRoot, Path.Combine(directory, $"{solutionName}.sln"));

        if (options.ScaffoldProjects)
        {
            ProjectScaffolding.Scaffold(directory, references);
        }

        return 0;
    }

    private static int RunDeriveAndReturnExitCode(DeriveOptions options)
    {
        var solutionSpec = SpecificationDeriver.DeriveFromSolution(options.SolutionPath, options.KeepMissingProjects);
        WriteSpecificationToFile(options.SolutionPath, solutionSpec);

        return 0;
    }

    private static int RunCleanupAndReturnExitCode(CleanupOptions options)
    {
        var solutionRoot = SpecificationDeriver.DeriveFromSolution(options.SolutionPath, options.KeepMissingProjects);
        var solutionId = Path.GetFileName(options.SolutionPath).DeriveGuid();
        if (options.CreateSpecificationFile)
        {
            WriteSpecificationToFile(options.SolutionPath, solutionRoot);
        }

        var references = solutionRoot.AggregateReferences(solutionId).ToArray();
        WriteSolutionToFile(solutionId, references, solutionRoot, options.SolutionPath);

        return 0;
    }

    private static SolutionRootDirectory ParseYamlSolutionSpecification(CreateOptions options)
    {
        var yamlSpecificationContent = File.ReadAllText(options.SolutionSpecPath);
        return new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build()
            .Deserialize<SolutionRootDirectory>(yamlSpecificationContent);
    }

    private static void WriteSolutionToFile(Guid solutionId, SolutionReference[] references, SolutionRootDirectory spec, string solutionPath)
    {
        var configuration = spec.Configurations ?? new List<string> { "Debug|Any CPU", "Release|Any CPU" };
        var solutionTextContent = SolutionGeneration.GenerateSolutionContent(solutionId, references, configuration);
        File.WriteAllText(solutionPath, solutionTextContent);
        Console.WriteLine($"Solution file '{solutionPath}' updated");
    }

    private static void WriteSpecificationToFile(string solutionPath, SolutionRootDirectory solutionSpec)
    {
        var yamlSpecificationPath = $"{solutionPath}.yaml";
        var yamlSpecificationContent = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitNull)
            .WithNamingConvention(UnderscoredNamingConvention.Instance).Build()
            .Serialize(solutionSpec);
        
        File.WriteAllText(yamlSpecificationPath, yamlSpecificationContent);
        Console.WriteLine($"Solution specification file '{yamlSpecificationPath}' created");
    }
}