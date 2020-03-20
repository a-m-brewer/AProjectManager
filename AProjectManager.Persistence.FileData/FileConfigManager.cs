using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AProjectManager.Persistence.FileData
{
    public class FileConfigManager : IFileConfigManager
    {
        private readonly IDataFolderProvider _folderProvider;
        private readonly IFileManager _fileManager;
        private readonly IConfigManager _configManager;
        private readonly string _configFolderPath;

        public FileConfigManager(
            IDataFolderProvider folderProvider,
            IFileManager fileManager,
            IConfigManager configManager)
        {
            _folderProvider = folderProvider;
            _fileManager = fileManager;
            _configManager = configManager;
            _configFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/aprojectmanager");
        }


        public T WriteData<T>(T obj, string fileName, params string[] subDirs)
        {
            fileName = fileName.EndsWith(".yml") ? fileName : $"{fileName}.yml";

            var path = new List<string>
            {
                _configFolderPath
            };
            
            path.AddRange(subDirs);

            var folderPath = Path.Combine(path.ToArray());
            
            var filePath = _folderProvider.GetPathOfFile(folderPath, fileName);
            
            var objString = _configManager.Serialize(obj);
            _fileManager.WriteData(filePath, objString);

            return obj;
        }

        public T GetFromFile<T>(string fileName, params string[] subDirs)
        {
            fileName = fileName.EndsWith(".yml") ? fileName : $"{fileName}.yml";
            
            var path = new List<string>
            {
                _configFolderPath
            };
            
            path.AddRange(subDirs);

            var folderPath = Path.Combine(path.ToArray());
            
            var filePath = _folderProvider.GetPathOfFile(folderPath, fileName);
            var stringData = _fileManager.GetData(filePath);
            return _configManager.Deserialize<T>(stringData);
        }

        public List<T> GetFilesInPath<T>(params string[] subDirs)
        {
            var path = new List<string>
            {
                _configFolderPath
            };
            
            path.AddRange(subDirs);

            var folderPath = Path.Combine(path.ToArray());

            var filePaths = Directory.GetFiles(folderPath, "*.yml");

            var allFileDataAsString = filePaths.Select(filePath => _fileManager.GetData(filePath));

            return allFileDataAsString
                .Select(fileDataAsString =>
                    _configManager.Deserialize<T>(fileDataAsString))
                .ToList();
        }
    }
}