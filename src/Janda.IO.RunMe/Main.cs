using Microsoft.Extensions.Logging;
using System;

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


        private void LogTreeException(Exception exception)
        {
            _logger.LogError(exception, "DirectoryTree error.");
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


            TreeEnumerate(path);
            TreeCallbacks(path);                         
        }
    }
}