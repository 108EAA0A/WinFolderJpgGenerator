using System;

namespace FolderThumbnailGenerator
{
    [Serializable]
    public class AppSettingNotFoundException : Exception
    {
        public AppSettingNotFoundException(string key) : base($"The application setting setting key \"{key}\" was not found.") { }
        public AppSettingNotFoundException(string key, Exception inner) : base($"The application setting setting key \"{key}\" was not found.", inner) { }
    }
}
