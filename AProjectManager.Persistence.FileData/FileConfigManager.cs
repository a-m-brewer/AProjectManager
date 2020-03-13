using System;
using System.IO;

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


        public T WriteData<T>(T obj, string fileName)
        {
            fileName = fileName.EndsWith(".yml") ? fileName : $"{fileName}.yml";
            var filePath = _folderProvider.GetPathOfFile(_configFolderPath, fileName);
            var objString = _configManager.Serialize(obj);
            _fileManager.WriteData(filePath, objString);

            return obj;
        }

        public T GetFromFile<T>(string fileName)
        {
            fileName = fileName.EndsWith(".yml") ? fileName : $"{fileName}.yml";
            var filePath = _folderProvider.GetPathOfFile(_configFolderPath, fileName);
            var stringData = _fileManager.GetData(filePath);
            return _configManager.Deserialize<T>(stringData);
        }
    }
}