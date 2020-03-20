using System.Collections.Generic;

namespace AProjectManager.Persistence.FileData
{
    public interface IFileConfigManager
    {
        T WriteData<T>(T obj, string fileName, params string[] subDirs);
        T GetFromFile<T>(string fileName, params string[] subDirs);
        List<T> GetFilesInPath<T>(params string[] subDirs);
    }
}