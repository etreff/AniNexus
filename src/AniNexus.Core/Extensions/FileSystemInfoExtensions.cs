namespace System.IO;

public static partial class FileSystemInfoExtensions
{
    /// <summary>
    /// Recursively deletes the files and directories within the <see cref="FileSystemInfo"/> object.
    /// </summary>
    /// <param name="fileSystemInfo">The file system info to delete the children of.</param>
    public static void DeleteChildren(this FileSystemInfo fileSystemInfo)
    {
        Guard.IsNotNull(fileSystemInfo, nameof(fileSystemInfo));

        if (fileSystemInfo is DirectoryInfo di)
        {
            di.Delete(true);
        }
    }

    /// <summary>
    /// Recursively deletes this <see cref="FileSystemInfo"/> and all files and directories in
    /// the <see cref="FileSystemInfo"/> object.
    /// </summary>
    /// <param name="fileSystemInfo">The file system info to delete.</param>
    public static void DeleteRecursive(this FileSystemInfo fileSystemInfo)
    {
        Guard.IsNotNull(fileSystemInfo, nameof(fileSystemInfo));

        DeleteChildren(fileSystemInfo);

        fileSystemInfo.Delete();
    }
}
