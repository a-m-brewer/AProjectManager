using System.Collections.Generic;

namespace AProjectManager.Domain.Git
{
    public class Remote
    {
        public string Name { get; set; }
        public IEnumerable<Repository> Repositories { get; set; }
    }
}