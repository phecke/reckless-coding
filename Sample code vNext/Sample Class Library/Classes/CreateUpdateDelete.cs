using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PowerPlatform.Dataverse.CodeSamples
{
    /// <summary>
    /// Demonstrates the Create, Update, Retrieve, and Delete service client methods.
    /// These methods are equivalent to the Create, Update, Retrieve, and Delete
    /// message requests.
    /// </summary>
    public class CreateUpdateDelete : IPowerSample
    {
        /// <summary>
        /// Creates an account, updates it, retrieves it, and then deletes it.
        /// Outputs the updated account name and postal code obtained from the
        /// web service to the console
        /// </summary>
        /// <param name="client">A configured Dataverse service client.</param>
        /// <param name="entityStore">Collection of entites created by this sample.</param>
        /// <returns>True if successfull; otherwise false</returns>
        /// <remarks>The entiy deletion is performed in IPowerSample.Cleanup().</remarks>
        public bool Run(ServiceClient client, EntityCollection entityStore)
        {
            // Create an in-memory account named First Coffee.
            Entity account = new("account");
            account["name"] = "First Coffee";

            // Create the account in Dataverse.
            account.Id = client.Create(account);
            entityStore.Entities.Add(account); // Save for later deletion.

            // In Dataverse, update the account's name and set it's postal code.
            account["name"] = "Fourth Coffee";
            account["address2_postalcode"] = "98052";
            client.Update(account);

            // Retrieve the updated account name and postal code from Dataverse.
            Entity retrievedAccount = client.Retrieve(
                entityName: account.LogicalName,
                id: account.Id,
                columnSet: new ColumnSet("name", "address2_postalcode")
            );

            Console.WriteLine("Retrieved account name: {0}, postal code: {1}",
                retrievedAccount["name"], retrievedAccount["address2_postalcode"]);

            return true;
        }
    }
}
