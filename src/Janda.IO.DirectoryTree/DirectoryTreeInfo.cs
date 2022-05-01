using System;
using System.IO;

namespace Janda.IO
{
    public sealed class DirectoryTreeInfo
    {
        private readonly int _rootLength = 0;

        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public int DirectoryLevel { get; set; }
        public string RootName { get; set; }
        public string DirectoryName { get; set; }
        public string Name { get; set; }
        public string FullName => Path.Combine(DirectoryName, Name);
        public string RelativeName => Path.Combine(DirectoryName[_rootLength..], Name);
        public FileAttributes Attributes { get; set; }
        public long Size { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastAccessTimeUtc { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }


        public DirectoryTreeInfo(string path)
        {
            Id = Guid.Empty;
            DirectoryLevel = -1;
            _rootLength = path.Length;
        }

        public DirectoryTreeInfo(string path, Guid id)
        {
            Id = id;
            DirectoryLevel = -1;
            _rootLength = path.Length;
        }

        public DirectoryTreeInfo(FileSystemInfo fsInfo, DirectoryTreeInfo dtInfo, string path, long size)
        {
            _rootLength = dtInfo._rootLength;

            Id = Guid.NewGuid();
            Parent = dtInfo.Id;
            DirectoryName = path;
            DirectoryLevel = dtInfo.DirectoryLevel + 1;
            Name = fsInfo.Name;
            Size = size;
            Attributes = fsInfo.Attributes;
            LastAccessTimeUtc = fsInfo.LastAccessTimeUtc;
            CreationTimeUtc = fsInfo.CreationTimeUtc;
            LastWriteTimeUtc = fsInfo.LastWriteTimeUtc;
        }
    }
}
