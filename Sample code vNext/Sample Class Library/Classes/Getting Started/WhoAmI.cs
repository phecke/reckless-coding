using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace GettingStarted
{
    /// <summary>
    /// Demonstrates executing a WhoAmI message request and response.
    /// </summary>
    public class WhoAmI: IPowerSample
    {
        /// <summary>
        /// Sends a WhoAmI request to the web service to obtain information
        /// about the logged on user. The user ID is returned in the response
        /// and output to the console.
        /// </summary>
        /// <param name="client">A configured Dataverse service client.</param>
        /// <param name="entityStore"></param>
        /// <returns>True if successfull; otherwise false</returns>
        public bool Run(ServiceClient client, EntityCollection entityStore)
        {
            WhoAmIResponse response =
                (WhoAmIResponse)client.Execute(new WhoAmIRequest());

            Console.WriteLine("User ID is {0}.", response.UserId);
            return (true);
        }
    }
}
