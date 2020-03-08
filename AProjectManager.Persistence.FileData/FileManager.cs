using System.IO;

namespace AProjectManager.Persistence.FileData
{
    public class FileManager : IFileManager
    {
        public string GetData(string path)
        {
            return !File.Exists(path) ? string.Empty : File.ReadAllText(path);
        }

        public void WriteData(string path, string data)
        {
            File.WriteAllText(path, data);
        }
    }
}