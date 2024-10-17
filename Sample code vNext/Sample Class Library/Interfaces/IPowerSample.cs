﻿using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace GettingStarted
{
    /// <summary>
    /// Defines an interface for a code sample that accesses Power Platform.
    /// </summary>
    /// <remarks>See ISample for a description of these methods.</remarks>
    public interface IPowerSample : ICloudSample
    {
        /// <summary>
        /// Allocates an empty entity collection.
        /// </summary>
        /// <param name="client">Configured web service client.</param>
        /// <returns>Collection of entity instances.</returns>
        public virtual EntityCollection Setup(ServiceClient client) { return new EntityCollection(); }

        /// <summary>
        /// Performs the main function of the code sample.
        /// </summary>
        /// <param name="client">Configured web service client.</param>
        /// <param name="entityStore">Collection of entity instances.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public virtual bool Run(ServiceClient client, EntityCollection entityStore) { return false; }

        /// <summary>
        /// Deletes any entity instances, in reverse order, stored in the collection.
        /// </summary>
        /// <param name="client">Configured web service client.</param>
        /// <param name="entityStore">Collection of entity instances.</param>
        public virtual void Cleanup(ServiceClient client, EntityCollection entityStore)
        {
            if (entityStore != null && entityStore.Entities.Count > 0)
            {
                Entity? entity;

                for(int i = entityStore.Entities.Count-1; i >= 0; i--)
                {
                    entity = entityStore.Entities[i];
                    client.Delete(entity.LogicalName, entity.Id);
                }
            }
        }
    }
}
