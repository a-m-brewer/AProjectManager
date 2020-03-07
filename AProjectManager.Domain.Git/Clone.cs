namespace AProjectManager.Domain.Git
{
    public class Clone
    {
        private Clone()
        {
            
        }

        public Repository Local { get; private set; }
        public Repository Remote { get; private set; }

        public static Clone Create(string localLocation, string remoteLocation, string name)
        {
            return new Clone
            {
                Local = new Repository
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