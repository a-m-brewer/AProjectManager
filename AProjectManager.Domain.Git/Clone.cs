namespace AProjectManager.Domain.Git
{
    public class Clone
    {
        private Clone()
        {
            
        }

        public IRepository Local { get; private set; }
        public IRepository Remote { get; private set; }

        public static Clone Create(string localLocation, string remoteLocation, string name)
        {
            return new Clone
            {
                Local = new LocalRepository
                {
                    Location = localLocation,
                    Name = name
                },
                Remote = new Repository
                {
                    Location = remoteLocation,
                    Name = name
                }
            };
        }
    }
}