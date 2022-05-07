using System.IO;

namespace Janda.IO
{
    public class DirectoryTreeFolder : DirectoryTreeInfo
    {
        public DirectoryTreeInfo[] Files { get; private set; }
        public DirectoryTreeFolder(string path) : base(path)
        {

        }

        public DirectoryTreeFolder(FileSystemInfo fsInfo, DirectoryTreeInfo dtInfo, string path, DirectoryTreeInfo[] files)
            : base(fsInfo, dtInfo, path, 0)
        {
            Files = files;
        }
    }
}
