using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class GroupAddRequest
    {
        public string GroupName { get; set; }

        public List<string> RepositorySlugs { get; set; }
    }
}