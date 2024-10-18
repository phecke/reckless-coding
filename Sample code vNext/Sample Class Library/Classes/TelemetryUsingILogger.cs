using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace PowerPlatform.Dataverse.CodeSamples
{
    /// <summary>
    /// Demonstrates executing a WhoAmI message request and response.
    /// </summary>
    public class TelemetryUsingILogger(IConfiguration config) : IPowerSample
    {
        private IConfiguration? _config = config;

        /// <summary>
        /// Sends a WhoAmI request to the web service to obtain information
        /// about the logged on user. The user ID is returned in the response
        /// and output to the console.
        /// </summary>
        /// <param name="client">A configured Dataverse service client.</param>
        /// <param name="entityStore">Collection of entites created by this sample.</param>
        /// <returns>True if successfull; otherwise false</returns>
        public bool Run(ServiceClient client, EntityCollection entityStore)
        {
            if(_config == null)
            {
                Console.WriteLine("Configuration is null.");
                return false;
            }
            TelemetryUsingILogger app = new(_config);

            // Add a logger using the 'Logging' configuration section in the
            // appsettings.json file, and send the logs to the console.
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddConsole()
                           .AddConfiguration(_config.GetSection("Logging")));

            // Dispose of the existing client and create a new one with logging.
            client.Dispose();
            client = new ServiceClient(
                dataverseConnectionString: _config.GetConnectionString("default"),
                logger: loggerFactory.CreateLogger<WhoAmI>());

            // Send a WhoAmI message request to the web service to obtain  
            // information about the logged on user.
            WhoAmIResponse resp = (WhoAmIResponse)client.Execute(new WhoAmIRequest());
            Console.WriteLine("\nUser ID is {0}.", resp.UserId);

            return true;
        }
    }
}
