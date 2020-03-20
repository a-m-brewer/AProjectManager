using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class DockerComposeSuperRequest
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public IEnumerable<string> Arguments { get; set; }
    }
}