namespace SimpleSolution.Core.Models;

public record SolutionReference(Guid ParentId, string ProjectName, string ProjectPath, Guid ProjectId);