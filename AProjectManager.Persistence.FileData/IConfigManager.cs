namespace AProjectManager.Persistence.FileData
{
    public interface IConfigManager
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string input);
    }
}