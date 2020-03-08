namespace AProjectManager.Persistence.FileData
{
    public interface IFileManager
    {
        string GetData(string path);
        void WriteData(string path, string data);
    }
}