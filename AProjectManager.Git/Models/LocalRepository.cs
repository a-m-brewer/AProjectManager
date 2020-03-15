using System;
using System.IO;

namespace AProjectManager.Git.Models
{
    public class LocalRepository : IRepository
    {
        private string _location;
        public string Name { get; set; }

        public  string Location
        {
            get => _location;
            set
            {
                if (value.StartsWith("~"))
                {
                    var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    var path = value.StartsWith("~/")
                        ? value.Replace("~/", "")
                        : value.Replace("~", "");
                    
                    _location = Path.Combine(homeFolder, path);
                    return;
                }

                _location = Path.GetFullPath(value);
            }
        }
    }
}