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


        private void LogTreeFolder(DirectoryTreeFolder info)
        {

            //_logger.LogInformation("[{id}]  [{parent}]  [{level}]  [{count}]\t[{item}]", info.Info.Id, info.Info.Parent, info.Info.DirectoryLevel, info.Files.Length, info.Info.Name);


            //_logger.LogInformation("[{id}]  [{parent}]  [{level}]\t[{item}]", info.Info.Id, info.Info.Parent, info.Info.DirectoryLevel, info.Info.RelativeName);
            //foreach (var file in info.Files)
            //    _logger.LogInformation(" {id}    {parent}    {level} \t {item} ", file.Id, file.Parent, file.DirectoryLevel, file.RelativeName);

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

            //path = @"C:\TEMP\DUPA";

            foreach (var folder in DirectoryTree.TraverseFolders(path, LogTreeException))
                LogTreeFolder(folder);

            _logger.LogInformation("Directory Tree Enumerator");

            //TreeEnumerate(path);
            //TreeCallbacks(path);                         
        }
    }
}