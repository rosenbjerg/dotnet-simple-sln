using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class SolutionGeneration
{
	public static string GenerateSolutionContent(Guid solutionId, IReadOnlyCollection<SolutionReference> references, List<string> configurations)
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
".TrimNewlines();
    }
    private static string GenerateProjects(IEnumerable<SolutionReference> references)
    {
	    return string.Join(Environment.NewLine, references.Select(GenerateProject));
    }

    private static string GenerateProject(SolutionReference reference)
    {
	    var projectTypeId = SupportedTypes.GetProjectTypeId(reference.ProjectPath);
	    return @$"
Project(""{{{projectTypeId}}}"") = ""{reference.ProjectName}"", ""{reference.ProjectPath}"", ""{{{(reference.ProjectId.FormatGuid())}}}""
EndProject
".TrimNewlines();
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
}