namespace AProjectManager.Persistence.FileData
{
    public interface IFileConfigManager
    {
        T WriteData<T>(T obj, string fileName, params string[] subDirs);
        T GetFromFile<T>(string fileName, params string[] subDirs);
    }
}