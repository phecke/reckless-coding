#define PREVIEW // SharedLink preview release. Limited set of tables supported.

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace PowerPlatform.Dataverse.CodeSamples
{
    /// <summary>
    /// Demonstrates table row shared link operations.
    /// </summary>
    /// <remarks>Set the appropriate Url and Username values for your test
    /// environment in the appsettings.json file before running this program.</remarks>
    /// <see cref="https://docs.microsoft.com/power-apps/developer/data-platform/xrm-tooling/use-connection-strings-xrm-tooling-connect#connection-string-parameters"/>
    /// <permission cref="https://github.com/microsoft/PowerApps-Samples/blob/master/LICENSE"
    /// <author>Peter Hecke</author>
    class Program
    {
        /// <summary>
        /// Contains the application's configuration settings. 
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Storage for table row references created by this program for later use.
        /// </summary>
        private static Dictionary<string,EntityReference> entityStore = new();

        /// <summary>
        /// Constructor. Loads the application configuration settings from a JSON file.
        /// </summary>
        Program()
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
            Program app = new();

            // Create a Dataverse service client using the default connection string.
            ServiceClient client =
                new(app.Configuration.GetConnectionString("default"));

            app.Setup(client); // Create initial required resources.

            // Obtain information about the table row created in Setuo().
            entityStore.TryGetValue("Fourth Coffee", out EntityReference? account);

            if( account != null )
            {
                // Enable link sharing on the table row, then create a shared link
                // with read-write access to the row.
                app.Enable_SharedLink(client, account);
                Console.WriteLine(
                    "Shared links enabled for {0} '{1}'", account.LogicalName, account.Name);

                Guid linkId = app.Create_SharedLink(client, account, 
                    AccessRights.ReadAccess | AccessRights.WriteAccess);
                Console.WriteLine(
                    "Shared link created for {0} '{1}'", account.LogicalName, account.Name);

                //Retrieve all shared links for the table row.
                EntityCollection rowSharedLinks = app.Retrieve_SharedLinks(client, account);
                Console.WriteLine(
                    "Shared links for {0} '{1}'", account.LogicalName, account.Name);
                for(int i=0; i<rowSharedLinks.TotalRecordCount; i++)
                {
                    Console.WriteLine("\tShared link [ID:{0}]", rowSharedLinks.Entities[i].Id);
                }

                // Revoke write access to the table row.
                app.Revoke_SharedLink(client, account, AccessRights.WriteAccess);
                Console.WriteLine("Write access revoked for shared link [ID:{0}].", linkId);
            }
            else
            {
                Console.WriteLine("Account was not found..exiting.");
            }

            // Pause program execution before resource cleanup.
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            app.Cleanup(client); // Delete any created resources.
            client.Dispose();
        }

        /// <summary>
        /// Creates any initial table rows or other resources required by the program.
        /// </summary>
        /// <param name="client">The service client.</param>
        /// <remarks>The entityStore is a global dictionary of table row references.</remarks>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.powerplatform.dataverse.client.serviceclient.create"/>
        public void Setup(ServiceClient client)
        {
            // A new account row.
            Guid id = client.Create(
                new Entity("Fourth Coffee") 
                    { LogicalName = "account" }
            );
            entityStore.Add("Fourth Coffee", new EntityReference("account", id));
        }

        /// <summary>
        /// Enables user access to a table row using a shared link.
        /// </summary>
        /// <param name="client">A ServiceClient instance.</param>
        /// <param name="entityId">The target table row.</param>
        /// <remarks>The entityStore is a global dictionary of table row references.</remarks>
        /// <see cref="https://docs.microsoft.com/power-apps/developer/data-platform/reference/entities/sharedlinksetting"/>
        public void Enable_SharedLink(ServiceClient client, EntityReference entRef)
        {
        #if PREVIEW
            if(entRef.LogicalName != "account"  && entRef.LogicalName != "contact" &&
               entRef.LogicalName != "case" && entRef.LogicalName != "opportunity")
            {
                throw new Exception(
                    string.Format("Table '{0}' does not currently support shared links.",
                    entRef.LogicalName));
            };
        #endif

            Entity row = new("sharedlinksetting");
            row.Attributes = new AttributeCollection()
            {
                // TODO Pass value as a lookup
                {"extensionofrecordid", entRef },
                {"isenabledforsharedlinkcreation", true }
            };

            Guid settingId = client.Create(row);
            entityStore.Add("shareLink", new EntityReference("sharedlinksetting", settingId));
        }

        /// <summary>
        /// Creates a shared link for a table row.
        /// </summary>
        /// <param name="client">A ServiceClient instance.</param>
        /// <param name="entRef">The target table row.</param>
        /// <param name="rights">Access rights for the table row.</param>
        /// <returns>Unique identifier of the shred link.</returns>
        /// <remarks>The entityStore is a global dictionary of table row references.</remarks>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.crm.sdk.messages.generatesharedlinkrequest"/>
        public Guid Create_SharedLink(ServiceClient client, EntityReference entRef,
            AccessRights rights)
        {
            GenerateSharedLinkRequest request = new()
            {
                Target = entRef,
                SharedRights = rights   
            };

            GenerateSharedLinkResponse response = 
                (GenerateSharedLinkResponse)client.Execute(request);

            EntityReference linkRef = new("shareLink", (Guid)response.Results["shareLink"]);
            entityStore.Add("shareLink", linkRef);

            return linkRef.Id;
        }

        /// <summary>
        /// Retrieve all shared links on the table row.
        /// </summary>
        /// <param name="client">A ServiceClient instance.</param>
        /// <param name="entRef">The target table row.</param>
        /// <returns>A collection of shared links for the table row.</returns>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.crm.sdk.messages.retrievesharedlinksrequest"/>
        public EntityCollection Retrieve_SharedLinks(ServiceClient client, 
            EntityReference entRef)
        {
            RetrieveSharedLinksRequest request = new() { Target = entRef };

            RetrieveSharedLinksResponse response =
                (RetrieveSharedLinksResponse)client.Execute(request);

            return response.SharedLinks;
        }

        /// <summary>
        /// Adds the calling user to the shared link access team of the table row.
        /// </summary>
        /// <param name="client">A ServiceClient instance.</param>
        /// <param name="entRef">The target table row.</param>
        /// <param name="linkId">The shared link ID.</param>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.crm.sdk.messages.grantaccessusingsharedlinkrequest"/>
        public void GrantAccessUsing_SharedLink(ServiceClient client, 
            EntityReference entRef, Guid linkId)
        {
            Uri uri = new(String.Format(
                "https://{0}/main.aspx?pagetype=entityrecord&etn={1}&id={2}&shareLink={3}",
                client.ConnectedOrgUriActual.Host,
                entRef.LogicalName, entRef.Id, linkId ));

            GrantAccessUsingSharedLinkRequest request = new()
            { RecordUrlWithSharedLink = uri.ToString() };

            GrantAccessUsingSharedLinkResponse response =
                (GrantAccessUsingSharedLinkResponse)client.Execute(request);
        }

        /// <summary>
        /// Revokes the specified user access rights from the shared link. 
        /// </summary>
        /// <param name="client">A ServiceClient instance.</param>
        /// <param name="entRef">The target table row.</param>
        /// <param name="rights">Access rights to revoke.</param>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.crm.sdk.messages.revokesharedlinkrequest"/>
        public void Revoke_SharedLink(ServiceClient client, EntityReference entRef,
            AccessRights rights)
        {
            RevokeSharedLinkRequest request = new()
            {
                Target = entRef,
                SharedRights = rights
            };
            
            RevokeSharedLinkResponse response = 
                (RevokeSharedLinkResponse)client.Execute(request);
        }

        /// <summary>
        /// Delete all table rows created by this program in reverse order of creation.
        /// </summary>
        /// <param name="client">The service client.</param>
        /// <remarks>The entityStore is a global dictionary of table row references.</remarks>
        /// <see cref="https://docs.microsoft.com/dotnet/api/microsoft.powerplatform.dataverse.client.serviceclient.delete"/>
        public void Cleanup(ServiceClient client)
        {
            while(entityStore.Count> 0)
            {
                KeyValuePair<string,EntityReference> kv = entityStore.Last();
                var key = kv.Key;  var entRef = kv.Value;

                try
                {
                    entityStore.Remove(key);
                    client.Delete(entRef.LogicalName, entRef.Id);
                }
                catch( Exception ex)
                {
                    Console.WriteLine(
                        "Table '{0}' row '{1}' could not be deleted..skipping.",
                        entRef.LogicalName, entRef.Name);
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}