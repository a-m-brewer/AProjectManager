using System.Threading.Tasks;
using CommandLine;

namespace AProjectManager.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<CliArguments>(args)
                .WithParsed(async passed => await new App().Run(passed))
                .WithNotParsed(failure => new AppError().HandleErrors(failure));

            // var config = new FileConfigManager(new DataFolderProvider(), new FileManager(), new YamlConfigManager(), "/Users/adambrewer");
            //
            // var loginManager = new BitBucketLoginManager(config, new BitBucketClient());
            //
            // await loginManager.Login(new AuthorizationCredentials {Key = "mkuRcCtmt7LTRHn95x", Secret = "rB9vkEMEwbb89Er38t2cEAFrawn2Gbut"});
        }
        
        /*
         * var client = new BitBucketClient();

            var token = await client.Authorize(new RefreshTokenRequest
            {
                AuthorizationCredentials = new AuthorizationCredentials
                {
                    Key = "mkuRcCtmt7LTRHn95x",
                    Secret = "rB9vkEMEwbb89Er38t2cEAFrawn2Gbut"
                },
                RefreshToken = "hXEGrb2wzQWLmFgkXC"
            });
            
            Console.WriteLine(token.AccessToken);
         */
        
        /*
         * var clone = Clone.Create("~/source/avoid-plugins",
                "http://avoid-network.xyz/a-m-brewer/avoid-plugins.git", "avoid-plugin");

            var process = new List<IProcess>
            {
                RepositoryManager.Clone(clone),
                RepositoryManager.Fetch((LocalRepository)clone.Local),
                RepositoryManager.Pull((LocalRepository)clone.Local),
                RepositoryManager.Checkout((LocalRepository)clone.Local, new Checkout { Branch = new Branch {Name = "testing"}, Create = true})
            };

            var runnable = process.ToRunnableProcess();

            runnable.Start();
         */
    }
}