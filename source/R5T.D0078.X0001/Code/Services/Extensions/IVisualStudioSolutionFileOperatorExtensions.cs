using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.Lombardy;

using R5T.D0078;
using R5T.T0114;

using Instances = R5T.D0078.X0001.Instances;


namespace System
{
    public static class IVisualStudioSolutionFileOperatorExtensions
    {
        public static Task AddDependencyProjectReference(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePathToModify,
            string dependencyProjectReferenceFilePathToAdd)
        {
            return visualStudioSolutionFileOperator.AddProjectReference(
                solutionFilePathToModify,
                dependencyProjectReferenceFilePathToAdd,
                Instances.SolutionFolderName.Dependencies());
        }

        public static Task AddDependencyProjectReferenceOkIfAlreadyAdded(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePathToModify,
            string dependencyProjectReferenceFilePathToAdd)
        {
            return visualStudioSolutionFileOperator.AddProjectReferenceOkIfAlreadyAdded(
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

        public static async Task AddDependencyProjectReferencesOfProjectReferences(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            params IProjectFileSpecification[] projectFileSpecifications)
        {
            var dependencyProjectReferenceFilePaths = projectFileSpecifications
                .SelectMany(xProjectFileSpecification => xProjectFileSpecification.DependencyProjectReferenceFilePaths)
                .Distinct()
                .OrderAlphabetically()
                .ToArray();

            foreach (var dependencyProjectReferenceFilePath in dependencyProjectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddDependencyProjectReference(
                    solutionFilePath,
                    dependencyProjectReferenceFilePath);
            }
        }

        /// <summary>
        /// Adds only the project reference file paths of the input <see cref="ProjectFileSpecification"/>s.
        /// </summary>
        public static async Task AddProjectReferencesOfProjectReferencesOnly(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            params IProjectFileSpecification[] projectFileSpecifications)
        {
            var projectReferenceFilePaths = projectFileSpecifications
                .SelectMany(xProjectFileSpecification => xProjectFileSpecification.ProjectReferenceFilePaths)
                .Distinct()
                .OrderAlphabetically()
                .ToArray();

            foreach (var projectReferenceFilePath in projectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddProjectReference(
                    solutionFilePath,
                    projectReferenceFilePath);
            }
        }

        /// <summary>
        /// Adds both the dependency project reference file paths and project reference file paths of the input <see cref="ProjectFileSpecification"/>.
        /// </summary>
        public static async Task AddProjectReferencesOfProjectReferences(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            params IProjectFileSpecification[] projectFileSpecifications)
        {
            await visualStudioSolutionFileOperator.AddProjectReferencesOfProjectReferencesOnly(
                solutionFilePath,
                projectFileSpecifications);

            await visualStudioSolutionFileOperator.AddDependencyProjectReferencesOfProjectReferences(
                solutionFilePath,
                projectFileSpecifications);
        }

        public static async Task CreateOnlyIfNotExists(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath)
        {
            var solutionFileExists = Instances.FileSystemOperator.FileExists(solutionFilePath);

            if (solutionFileExists)
            {
                return;
            }

            await visualStudioSolutionFileOperator.Create(solutionFilePath);
        }

        public static async Task Create(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath)
        {
            var solutionDirectoryPath = Instances.SolutionPathsOperator.GetSolutionDirectoryPath(solutionFilePath);

            var solutionName = Instances.SolutionPathsOperator.GetSolutionName(solutionFilePath);

            await visualStudioSolutionFileOperator.Create(solutionName, solutionDirectoryPath);
        }

        public static async Task<string[]> ListProjectReferencePaths(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var solutionDirectoryPath = stringlyTypedPathOperator.GetDirectoryPathForFilePath(solutionFilePath);

            var projectReferenceRelativePaths = await visualStudioSolutionFileOperator.ListProjectReferenceRelativePaths(solutionFilePath);

            var output = projectReferenceRelativePaths
                .Select(xRelativeFilePath => stringlyTypedPathOperator.Combine(solutionDirectoryPath, xRelativeFilePath))
                .ToArray();

            return output;
        }

        public static async Task<bool> HasProjectReference(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            string projectReferenceFilePath,
            IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var projectReferencePaths = await visualStudioSolutionFileOperator.ListProjectReferencePaths(
                solutionFilePath,
                stringlyTypedPathOperator);

            var output = projectReferencePaths.Contains(projectReferenceFilePath);
            return output;
        }

        public static async Task<bool> RemoveProjectReferenceOkIfNotExists(this IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            string solutionFilePath,
            string projectReferenceFilePath,
            IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var hasProjectReference = await visualStudioSolutionFileOperator.HasProjectReference(
                solutionFilePath,
                projectReferenceFilePath,
                stringlyTypedPathOperator);

            if (hasProjectReference)
            {
                await visualStudioSolutionFileOperator.RemoveProjectReferenceIdempotent(solutionFilePath, projectReferenceFilePath);
            }

            return hasProjectReference;
        }
    }
}
