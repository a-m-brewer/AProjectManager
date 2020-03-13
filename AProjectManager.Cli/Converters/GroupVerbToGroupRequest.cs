using System.Linq;
using AProjectManager.Cli.Models;
using AProjectManager.Models;

namespace AProjectManager.Cli.Converters
{
    public static class GroupVerbToGroupRequest
    {
        public static GroupAddRequest ToAdd(this GroupVerb verb)
        {
            return new GroupAddRequest
            {
                GroupName = verb.GroupName,
                ProjectLocations = verb.Projects.ToList()
            };
        }
    }
}