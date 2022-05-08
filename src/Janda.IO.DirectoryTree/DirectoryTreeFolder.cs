using System.IO;
using System.Linq;

namespace Janda.IO
{
    public sealed class DirectoryTreeFolder
    {
        public DirectoryTreeInfo[] Files { get; internal set; }
        public DirectoryTreeInfo Info { get; internal set; }


        public DirectoryTreeFolder(DirectoryTreeInfo dtInfo, FileSystemInfo[] fsItems, string path)
        {
            Info = dtInfo;
            Files = fsItems.OfType<FileInfo>().Select(a => new DirectoryTreeInfo(a, dtInfo, path, a.Length)).ToArray();
        }
    }
}
