namespace AProjectManager.Constants
{
    public static class ConfigFiles
    {
        public const string Token = nameof(Token);
        public const string User = nameof(User);

        public static string RepoConfigName(string service, string name)
        {
            return $"{service}-{name}-repos";
        }
    }
}