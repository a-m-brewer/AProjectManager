using System.IO;

namespace AProjectManager.Persistence.FileData
{
    public class DataFolderProvider : IDataFolderProvider
    {
        public string GetDirectory(string path)
        {
            return Directory.CreateDirectory(path).FullName;
        }

        public string GetPathOfFile(string folderPath, string fileName)
        {
            return Path.Combine(GetDirectory(folderPath), fileName);
        }
    }
}