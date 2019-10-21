using Newtonsoft.Json;
using System;
using System.IO;

namespace QueueView.Configuration
{
    public class JsonConfigurationStore : IConfigurationStore
    {
        /// <summary>
        /// The name of the configuration file on the file system.
        /// </summary>
        public const string FileName = "QueueView.json";

        /// <summary>
        /// The fully qualified path for the file on the file system.
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// Constructs a new <see cref="JsonConfigurationStore"/> with a path to config file in the
        /// user's home folder.
        /// </summary>
        public JsonConfigurationStore()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string separator = Path.DirectorySeparatorChar.ToString();

            _fileName = $"{homeDirectory}{separator}{FileName}";
        }

        /// <inheritdoc cref="IConfigurationStore.ReadConfiguration"/>
        public Models.Configuration ReadConfiguration()
        {
            try
            {
                using (StreamReader stream = new StreamReader(_fileName))
                {
                    return JsonConvert.DeserializeObject<Models.Configuration>(stream.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                return new Models.Configuration();
            }
        }

        /// <inheritdoc cref="IConfigurationStore.WriteConfiguration"/>
        public void WriteConfiguration(Models.Configuration configuration)
        {
            using (StreamWriter stream = new StreamWriter(_fileName))
            {
                stream.Write(JsonConvert.SerializeObject(configuration));
            }
        }
    }
}
