namespace PowerPlatform.Dataverse.CodeSamples
{
    /// <summary>
    /// Defines an interface for a code sample.
    /// </summary>
    public interface ISample
    {
        /// <summary>
        /// Performs any setup or resource allocation required by but not performed in the Run() method.
        /// </summary>
        /// <returns>Created data or other allocated resource.</returns>
        public virtual object? Setup() { return null; }

        /// <summary>
        /// Performs the main function of the code sample.
        /// </summary>
        /// <param name="resource">Data or other objects required but not created in this method.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public virtual bool Run(object? resource) { return false; }

        /// <summary>
        /// Frees up any resources allocated by the Setup() method.
        /// </summary>
        /// <param name="resource">Allocated resources.</param>
        public virtual void Cleanup(object? resource) { }
    }
}
