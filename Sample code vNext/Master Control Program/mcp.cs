using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace PowerPlatform.Dataverse.CodeSamples
{
    /// <summary>
    /// Master control program level 1 (basic).
    /// </summary> 
    internal class mcp
    {
        /// <summary>
        /// Contains the application's configuration settings. 
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor. Loads the application configuration settings from a JSON file.
        /// </summary>
        mcp()
        {
            // Get the path to the appsettings file. If the environment variable is set,
            // use that file path. Otherwise, use the runtime folder's settings file.
            string? path = Environment.GetEnvironmentVariable("DATAVERSE_APPSETTINGS");
            if (path == null) path = "appsettings.json";

            // Load the app's configuration settings from the JSON file.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(path, optional: false, reloadOnChange: true)
                .Build();
        }

        static void Main(string[] args)
        {
            mcp mcpApp = new();
            IPowerSample app = new CreateUpdateDelete();

            // Create a web service client using the default connection string.
            ServiceClient serviceClient = 
                new(mcpApp.Configuration.GetConnectionString("default"));

            EntityCollection entityStore = app.Setup(serviceClient);

            if (app.Run(serviceClient, entityStore) == false)
                Console.WriteLine( typeof(CreateUpdateDelete)
                    + ".Run() method did not complete successfully.");

            app.Cleanup(serviceClient, entityStore);

            // Pause the console so it does not close.
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}
