using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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




        

        public static IEnumerable<DirectoryTreeFolder> TraverseFolders(DirectoryTreeInfo dtInfo, string path, Action<Exception> exception)
        {

            FileSystemInfo[] treeItems = null;
            DirectoryInfo dirItem = null;
            DirectoryTreeInfo[] files = Array.Empty<DirectoryTreeInfo>();

            try
            {
                dirItem = new DirectoryInfo(path);
                treeItems = dirItem.GetFileSystemInfos();
                files = treeItems.OfType<FileInfo>().Select(a => new DirectoryTreeInfo(a, dtInfo, path, a.Length)).ToArray();
            }
            catch (Exception ex)
            {
                exception?.Invoke(ex);
            }

            yield return new DirectoryTreeFolder(dirItem, dtInfo, path, files);

            //foreach (var dirInfo in treeItems?.OfType<DirectoryInfo>())
            //{             
            //    var treeDir = new DirectoryTreeInfo(dirInfo, dtInfo, path, 0);
            //    yield return new DirectoryTreeFolder(dirItem, dtInfo, path, files);

            //    foreach (var item in TraverseFolders(treeDir, Path.Combine(path, dirInfo.Name), exception))
            //        yield return item;
            //}


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


        public static IEnumerable<DirectoryTreeFolder> TraverseFolders(string path, Action<Exception> exception) => TraverseFolders(GetFirstTreeInfo(path), AddPathSeparator(path), exception);
        public static IEnumerable<DirectoryTreeFolder> TraverseFolders(string path) => TraverseFolders(GetFirstTreeInfo(path), AddPathSeparator(path), null);

    }
}
