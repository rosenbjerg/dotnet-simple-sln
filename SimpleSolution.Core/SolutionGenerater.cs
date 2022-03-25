using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimpleSolution.Core;

public static class ProjectScaffolder
{
	private static readonly string ProjectContent = @"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

</Project>
".TrimNewlines();

	public static void Scaffold(string directory, IEnumerable<SolutionReference> references)
	{
		Console.WriteLine("Scaffolding missing projects");
		var projects = references.Where(r => r.ProjectName != r.ProjectPath);
		foreach (var p in projects)
		{
			var path = Path.Combine(directory, p.ProjectPath.AlignDirectorySeparators());
			if (File.Exists(path))
			{
				continue;
			}

			var pathDirectory = Path.GetDirectoryName(path);
			if (pathDirectory != directory)
			{
				Directory.CreateDirectory(pathDirectory!);
			}

			Console.WriteLine($"Project '{path}' has been scaffolded");
			File.WriteAllText(path, ProjectContent);
		}
	}
}
public static class SolutionGenerater
{
	private const string SolutionDirectoryTypeId = "2150E333-8FDC-42A3-9474-1A3956D46DE8";
	private const string CSharpProjectTypeId = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
	private const string FSharpProjectTypeId = "F2A71F9B-5D33-465A-A702-920D77279786";
	
    public static string? GenerateSolution(Guid solutionId, IReadOnlyCollection<SolutionReference> references, List<string> configurations)
    {
	    return @$"
Microsoft Visual Studio Solution File, Format Version 12.00

{GenerateProjects(references)}
Global
{GenerateGlobalSolutionConfigurationPlatforms(configurations)}
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
{GenerateGlobalProjectConfigurationPlatforms(references, configurations)}
{GenerateGlobalNestedProjects(references, solutionId)}
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {{{solutionId}}}
	EndGlobalSection
EndGlobal
";
    }
    private static string GenerateProjects(IEnumerable<SolutionReference> references)
    {
	    return string.Join(Environment.NewLine, references.Select(r => @$"
Project(""{{{ExtensionToTypeId(Path.GetExtension(r.ProjectPath))}}}"") = ""{r.ProjectName}"", ""{r.ProjectPath}"", ""{{{(r.ProjectId.FormatGuid())}}}""
EndProject
".Trim()));
    }

    private static string GenerateGlobalSolutionConfigurationPlatforms(IEnumerable<string> configurations)
    {
	    return @$"
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
{string.Join(Environment.NewLine, configurations.Select(c => $"		{c} = {c}"))}
	EndGlobalSection
".TrimNewlines();
    }

    private static string GenerateGlobalNestedProjects(IEnumerable<SolutionReference> references, Guid solutionId)
    {
	    return @$"
	GlobalSection(NestedProjects) = preSolution
{string.Join(Environment.NewLine, references.Where(r => r.ParentId != solutionId).Select(r => $"		{{{(r.ProjectId.FormatGuid())}}} = {{{(r.ParentId.FormatGuid())}}}"))}
	EndGlobalSection
".TrimNewlines();
    }

    private static string GenerateGlobalProjectConfigurationPlatforms(IEnumerable<SolutionReference> references, IReadOnlyCollection<string> configurations)
    {
	    var projectReferences = references.Where(r => Path.GetExtension(r.ProjectPath).ToLowerInvariant() is ".csproj" or ".fsproj");
        return @$"
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
{string.Join(Environment.NewLine, projectReferences.Select(p => GenerateProjectConfigurations(p.ProjectId, configurations)))}
	EndGlobalSection
".TrimNewlines();
    }

    private static string GenerateProjectConfigurations(Guid projectGuid, IEnumerable<string> configurations)
    {
        return string.Join(Environment.NewLine, configurations.Select(c => @$"
		{{{(projectGuid.FormatGuid())}}}.{c}.ActiveCfg = {c}
		{{{(projectGuid.FormatGuid())}}}.{c}.Build.0 = {c}
".TrimNewlines()));
    }

    private static string ExtensionToTypeId(string extension)
    {
	    return extension.Trim('.') switch
	    {
		    "csproj" => CSharpProjectTypeId,
		    "fsproj" => FSharpProjectTypeId,
		    _ => SolutionDirectoryTypeId
	    };
    }
}