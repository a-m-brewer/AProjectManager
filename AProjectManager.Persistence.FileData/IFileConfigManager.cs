namespace AProjectManager.Persistence.FileData
{
    public interface IFileConfigManager
    {
        void WriteData<T>(T obj, string fileName);
        T GetFromFile<T>(string fileName);
    }
}