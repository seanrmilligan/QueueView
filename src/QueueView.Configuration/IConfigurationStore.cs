namespace QueueView.Configuration
{
    public interface IConfigurationStore
    {
        /// <summary>
        /// Load the configuration settings from their store.
        /// </summary>
        /// <returns>A <see cref="Models.Configuration"/>.</returns>
        Models.Configuration ReadConfiguration();

        /// <summary>
        /// Write the configuration settings to their store.
        /// </summary>
        /// <param name="configuration">The configuration settings to store.</param>
        void WriteConfiguration(Models.Configuration configuration);
    }
}
