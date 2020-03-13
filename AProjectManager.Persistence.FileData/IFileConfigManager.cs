namespace AProjectManager.Persistence.FileData
{
    public interface IFileConfigManager
    {
        T WriteData<T>(T obj, string fileName);
        T GetFromFile<T>(string fileName);
    }
}