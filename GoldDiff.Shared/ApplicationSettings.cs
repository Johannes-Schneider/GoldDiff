using System;
using System.IO;
using Newtonsoft.Json;

namespace GoldDiff.Shared
{
    public class ApplicationSettings
    {
        private static string StorageLocation { get; } = Path.Combine(Environment.CurrentDirectory, $"{nameof(ApplicationSettings)}.json");

        public static ApplicationSettings Load()
        {
            try
            {
                return JsonConvert.DeserializeObject<ApplicationSettings>(File.ReadAllText(StorageLocation));
            }
            catch
            {
                return new ApplicationSettings
                       {
                           ThemeLocation = "pack://application:,,,/GoldDiff.Shared;component/View/Theme/Default.xaml",
                       };
            }
        }
        
        public string ThemeLocation { get; protected set; } = string.Empty;

        public void Save()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(StorageLocation)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(StorageLocation)!);
                }

                File.WriteAllText(StorageLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception exception)
            {
                // TODO: implement error handling
            }
        }
    }
}