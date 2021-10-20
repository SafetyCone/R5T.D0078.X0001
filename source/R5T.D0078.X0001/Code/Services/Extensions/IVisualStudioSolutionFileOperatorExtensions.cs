using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R5T.D0078;

using Instances = R5T.D0078.X0001.Instances;


namespace System
{
    public static class IVisualStudioSolutionFileOperatorExtensions
    {
        public static Task AddDependencyProjectReference(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePathToModify,
            string dependencyProjectReferenceFilePathToAdd)
        {
            return visualStudioSolutionFileOperator.AddProjectReferences(
                solutionFilePathToModify,
                dependencyProjectReferenceFilePathToAdd,
                Instances.SolutionFolderName.Dependencies());
        }

        public static Task AddDependencyProjectReferences(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePathToModify,
            IEnumerable<string> dependencyProjectReferenceFilePathsToAdd)
        {
            return visualStudioSolutionFileOperator.AddProjectReferences(
                solutionFilePathToModify,
                dependencyProjectReferenceFilePathsToAdd,
                Instances.SolutionFolderName.Dependencies());
        }
    }
}
