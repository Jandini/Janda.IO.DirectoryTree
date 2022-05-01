using System;
using System.Collections.Generic;
using System.IO;

namespace Janda.IO
{
    public sealed class DirectoryTree
    {
        internal static void Traverse(DirectoryTreeInfo dtInfo, string path, Action<DirectoryTreeInfo> dir, Action<DirectoryTreeInfo> file, Action<Exception> exception)
        {
            try
            {
                foreach (var treeItem in new DirectoryInfo(path).GetFileSystemInfos())
                {
                    switch (treeItem)
                    {
                        case DirectoryInfo dirInfo:
                            var treeDir = new DirectoryTreeInfo(dirInfo, dtInfo, path, 0);
                            dir?.Invoke(treeDir);
                            Traverse(treeDir, Path.Combine(path, treeItem.Name), dir, file, exception);
                            break;

                        case FileInfo fileInfo:
                            if (file is not null)
                                file.Invoke(new DirectoryTreeInfo(fileInfo, dtInfo, path, fileInfo.Length));
                            break;

                        default:
                            throw new NotSupportedException();

                    };
                }
            }
            catch (Exception ex)
            {
                exception?.Invoke(ex);
            }
        }



        internal static IEnumerable<DirectoryTreeInfo> Traverse(DirectoryTreeInfo dtInfo, string path, Action<Exception> exception)
        {

            FileSystemInfo[] treeItems = null;

            try
            {
                treeItems = new DirectoryInfo(path).GetFileSystemInfos();
            }
            catch (Exception ex)
            {
                exception?.Invoke(ex);
            }

            if (treeItems is not null)
            {
                foreach (var treeItem in treeItems)
                {
                    switch (treeItem)
                    {
                        case DirectoryInfo dirInfo:
                            var treeDir = new DirectoryTreeInfo(dirInfo, dtInfo, path, 0);
                            yield return treeDir;

                            foreach (var item in Traverse(treeDir, Path.Combine(path, dirInfo.Name), exception))
                                yield return item;

                            break;

                        case FileInfo fileInfo:
                            yield return new DirectoryTreeInfo(fileInfo, dtInfo, path, fileInfo.Length);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }


        private static string AddPathSeparator(string path) => !path.EndsWith(Path.DirectorySeparatorChar) ? path += Path.DirectorySeparatorChar : path;
        private static DirectoryTreeInfo GetFirstTreeInfo(string path) => new(AddPathSeparator(path));
        private static DirectoryTreeInfo GetFirstTreeInfo(string path, Guid parent) => new(AddPathSeparator(path), parent);



        public static void Traverse(string path, Action<DirectoryTreeInfo> dir, Action<DirectoryTreeInfo> file, Action<Exception> exception) => Traverse(GetFirstTreeInfo(path), AddPathSeparator(path), dir, file, exception);
        public static void Traverse(string path, Guid parent, Action<DirectoryTreeInfo> dir, Action<DirectoryTreeInfo> file, Action<Exception> exception) => Traverse(GetFirstTreeInfo(path, parent), AddPathSeparator(path), dir, file, exception);


        public static IEnumerable<DirectoryTreeInfo> Traverse(string path, Action<Exception> exception) => Traverse(GetFirstTreeInfo(path), AddPathSeparator(path), exception);
        public static IEnumerable<DirectoryTreeInfo> Traverse(string path, Guid parent, Action<Exception> exception) => Traverse(GetFirstTreeInfo(path, parent), AddPathSeparator(path), exception);
        public static IEnumerable<DirectoryTreeInfo> Traverse(string path) => Traverse(GetFirstTreeInfo(path), AddPathSeparator(path), null);
        public static IEnumerable<DirectoryTreeInfo> Traverse(string path, Guid parent) => Traverse(GetFirstTreeInfo(path, parent), AddPathSeparator(path), null);
    }
}
