using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Janda.IO.RunMe
{
    internal class Main : IMain
    {
        private readonly ILogger<Main> _logger;

        public Main(ILogger<Main> logger)
        {
            _logger = logger;
        }

        
        private void LogTreeInfo(DirectoryTreeInfo info)
        {
            _logger.LogInformation("{id}  {parent}  {level}\t{item}", info.Id, info.Parent, info.DirectoryLevel, info.RelativeName);
        }


        private static byte[] GetHash(string value) => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value));

        private static IEnumerable<byte[]> FileHashes(DirectoryTreeInfo[] files)
        {
            foreach (var file in files)
                yield return GetHash(file.Name + file.Size);
        }

        private void LogTreeFolder(DirectoryTreeFolder folder)
        {
            //return;


            var hashes = new StringBuilder();

            foreach (var fh in FileHashes(folder.Files).OrderBy(a => a ?? new byte[0]))
                hashes.Append(fh);


            var hash = GetHash(hashes.ToString());

            _logger.LogInformation("[{id}]  [{parent}]  [{hash}]  [{level}]  [{count}]\t[{item}]", folder.Info.Id, folder.Info.Parent, hash, folder.Info.DirectoryLevel, folder.Files.Length, folder.Info.Name);



            //_logger.LogInformation("[{id}]  [{parent}]  [{level}]\t[{item}]", info.Info.Id, info.Info.Parent, info.Info.DirectoryLevel, info.Info.RelativeName);
            //foreach (var file in folder.Files)
            //{


            //    _logger.LogInformation(" {id}    {parent}    {level} \t {item} {hash}", file.Id, file.Parent, file.DirectoryLevel, file.Name, hash);


            //}
            //    _logger.LogInformation(" {id}    {parent}    {level} \t {item} ", file.Id, file.Parent, file.DirectoryLevel, file.Name);

            //_logger.LogInformation("-----------------------------------------------------------------------------------------");

        }


        private void LogTreeException(Exception exception)
        {
            _logger.LogError(exception.Message);
        }


        private void TreeEnumerate(string path)
        {
            foreach (var info in DirectoryTree.Traverse(path, LogTreeException))
                LogTreeInfo(info);
        }


        private void TreeCallbacks(string path)
        {
            DirectoryTree.Traverse(path, dir => { LogTreeInfo(dir); }, file => { LogTreeInfo(file); }, LogTreeException);
        }


        public void Run() 
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            path = @"C:\TEMP\DUPA";

            foreach (var folder in DirectoryTree.TraverseFolders(path, LogTreeException))
                LogTreeFolder(folder);

            _logger.LogInformation("Directory Tree Enumerator");

            //TreeEnumerate(path);
            //TreeCallbacks(path);                         
        }
    }
}