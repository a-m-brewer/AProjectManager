namespace AProjectManager.Models
{
    public class DockerComposeDownRequest
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public bool Build { get; set; }
    }
}