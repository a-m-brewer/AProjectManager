namespace AProjectManager.Persistence.FileData
{
    public interface IDataFolderProvider
    {
        string GetDirectory(string path);
        string GetPathOfFile(string folderPath, string fileName);
    }
}