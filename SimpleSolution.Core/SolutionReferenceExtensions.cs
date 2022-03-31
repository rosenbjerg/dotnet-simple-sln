using SimpleSolution.Core.Models;

namespace SimpleSolution.Core;

public static class SolutionReferenceExtensions
{
    public static bool IsProjectDirectory(this SolutionReference solutionReference)
    {
        return solutionReference.ProjectName == solutionReference.ProjectPath;
    }
}