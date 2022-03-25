using System;
using System.IO;
using System.Linq;
using SimpleSolution.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var specificationFilePath = args.FirstOrDefault() ?? "Test.sln.yaml";
var scaffold = args.Contains("--scaffold") || true;

var started = DateTime.UtcNow;

Console.WriteLine($"Reading solution specification file: {specificationFilePath}");
var directory = Path.GetDirectoryName(specificationFilePath) ?? ".";
var solutionName = Path.GetFileNameWithoutExtension(specificationFilePath).Replace(".sln", "");

var yamlSpec = await File.ReadAllTextAsync(specificationFilePath);
var spec = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build().Deserialize<SolutionRoot>(yamlSpec);

var solutionId = solutionName.DeriveGuid();
var references = spec.AggregateReferences(solutionId).ToArray();

Console.WriteLine("Generating sln file content");
var slnContent = SolutionGenerater.GenerateSolution(solutionId, references, spec.Configurations);
var outputPath = Path.Combine(directory, $"{solutionName}.sln");
await File.WriteAllTextAsync(outputPath, slnContent);
Console.WriteLine($"Solution file '{outputPath}' updated");

if (scaffold)
{
    ProjectScaffolder.Scaffold(directory, references);
}

var elapsed = DateTime.UtcNow.Subtract(started).TotalMilliseconds;
Console.WriteLine($"Elapsed: {elapsed}ms");