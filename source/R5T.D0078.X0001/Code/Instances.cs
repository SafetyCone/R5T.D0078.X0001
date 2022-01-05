using System;

using R5T.T0040;
using R5T.T0044;
using R5T.T0059;


namespace R5T.D0078.X0001
{
    public static class Instances
    {
        public static IFileSystemOperator FileSystemOperator { get; } = T0044.FileSystemOperator.Instance;
        public static ISolutionFolderName SolutionFolderName { get; } = T0059.SolutionFolderName.Instance;
        public static ISolutionPathsOperator SolutionPathsOperator { get; } = T0040.SolutionPathsOperator.Instance;
    }
}
