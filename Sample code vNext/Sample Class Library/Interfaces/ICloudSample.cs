namespace GettingStarted
{
    /// <summary>
    /// Defines an interface for a code sample that accesses a cloud service.
    /// </summary>
    /// <remarks>See ISample for a description of these methods.</remarks>
    public interface ICloudSample : ISample
    {
        /// <summary>
        /// Create any resources the app needs.
        /// </summary>
        /// <param name="client">A configured HTTP client.</param>
        /// <returns>Allocated resources.</returns>
        public virtual Dictionary<string, object>? Setup(HttpClient client) 
            { return null; }

        /// <summary>
        /// Performs the main function of the code sample.
        /// </summary>
        /// <param name="client">A configured HTTP client.</param>
        /// <param name="collection">Allocated resources.</param>
        /// <returns>True is successfull; otherwise false.</returns>
        public virtual bool Run(HttpClient client, Dictionary<string, object>? collection)
            { return false; }

        /// <summary>
        /// Frees any allocated resources.
        /// </summary>
        /// <param name="client">A configured HTTP client.</param>
        /// <param name="collection">Allocated resources.</param>
        public virtual void Cleanup(HttpClient client, Dictionary<string, object>? collection) { }
    }
}
