namespace FolderThumbnailGenerator
{
    public static class AppSettings
    {
        public static T Get<T>(string key)
        {
            var appSetting = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new AppSettingNotFoundException(key);

            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }
}
