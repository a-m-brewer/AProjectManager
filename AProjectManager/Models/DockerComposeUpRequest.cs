namespace AProjectManager.Models
{
    public class DockerComposeUpRequest
    {
        public string Name { get; set; }
        public bool Build { get; set; }
        public string FileName { get; set; }
    }
}