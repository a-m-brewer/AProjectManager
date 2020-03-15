using System.Collections.Generic;
using System.Linq;

namespace AProjectManager.Extensions
{
    public static class RepositoryRegisterExtensions
    {
        public static (List<string> existingSlugs, List<string> nonExistentSlugs) SplitExistingAndNonExisting(
            this Dictionary<string, bool> slugs)
        {
            var repositoriesThatDoNotExist = slugs.Where(w => !w.Value).Select(s => s.Key).ToList();
            var repositoriesThatExist = slugs.Where(w => w.Value).Select(s => s.Key).ToList();

            return (repositoriesThatExist, repositoriesThatDoNotExist);
        }
    }
}