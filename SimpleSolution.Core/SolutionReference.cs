using System;

namespace SimpleSolution.Core;

public record SolutionReference(Guid ParentId, string ProjectName, string ProjectPath, Guid ProjectId);