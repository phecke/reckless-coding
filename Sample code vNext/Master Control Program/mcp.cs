using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using GettingStarted;

namespace Master_Control_Program
{
    /// <summary>
    /// Master control program level 1 (basic).
    /// </summary>
    internal class mcp
    {
        #region Cloud service connection
        // TODO Place this info in a settings.json file.
        static string url = "https://myorg.crm.dynamics.com";
        static string userName = "someone@myorg.onmicrosoft.com";
        static string password = "mypassword";

        // This service connection string uses the info provided above.
        // The AppId and RedirectUri used here are provided for sample code testing.
        static string connectionString = $@"
            AuthType = OAuth;
            Url = {url};
            UserName = {userName};
            Password = {password};
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            LoginPrompt=Auto;
            RequireNewInstance = True";
        #endregion Cloud service connection

        static void Main(string[] args)
        {
            try
            {
                // Connect to the Dataverse web service.
                ServiceClient client = new ServiceClient(connectionString);   

                try
                {
                    IPowerSample app = new CreateUpdateDelete();
                    EntityCollection entityStore = app.Setup(client);

                    if (app.Run(client, entityStore) == false)
                        Console.WriteLine(typeof(CreateUpdateDelete) + 
                            ".Run() method did not complete successfully.");

                    app.Cleanup(client, entityStore);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(typeof(CreateUpdateDelete) + " terminated with an exception.");
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MCP was unable to establish a connection with the web service");
                Console.WriteLine(ex.Message);
            }

            // Pause the console so it does not close.
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}
