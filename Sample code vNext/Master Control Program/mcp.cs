using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using GettingStarted;
using Microsoft.Extensions.Configuration; 

namespace Master_Control_Program
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
            EntityCollection entityStore;
            IPowerSample app;

            mcp mcpApp = new();

            try
            {
                // Create a Dataverse service client using the default connection string.
                ServiceClient serviceClient =
                   new(mcpApp.Configuration.GetConnectionString("default"));

                app = new CreateUpdateDelete();
                entityStore = app.Setup(serviceClient);

                try
                {
                    if (app.Run(serviceClient, entityStore) == false)
                        Console.WriteLine(typeof(CreateUpdateDelete) +
                                ".Run() method did not complete successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(typeof(CreateUpdateDelete) + " terminated with an exception.");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    app.Cleanup(serviceClient, entityStore);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The program unable to establish a connection with the Dataverse web service.");
                Console.WriteLine(ex.Message);
            }

            // Pause the console so it does not close.
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}
