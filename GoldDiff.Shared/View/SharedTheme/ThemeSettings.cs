using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using FlatXaml.Model;
using Newtonsoft.Json;

namespace GoldDiff.Shared.View.SharedTheme
{
    public class ThemeSettings : ViewModel
    {
        private static string StorageLocation { get; } = Path.Combine(Environment.CurrentDirectory, "Config", "Theme.json");

    #region Singleton

        private static ThemeSettings? _instance;

        public static ThemeSettings Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                try
                {
                    _instance = JsonConvert.DeserializeObject<ThemeSettings>(File.ReadAllText(StorageLocation));
                }
                catch
                {
                    _instance = new ThemeSettings();
                }

                return _instance;
            }
        }

        private ThemeSettings() { }

    #endregion
        
        private ThemeType _theme = ThemeType.Light;

        public ThemeType Theme
        {
            get => _theme;
            set => MutateVerbose(ref _theme, value);
        }

        public IEnumerable<ResourceDictionary> LoadTheme()
        {
            switch (Theme)
            {
                case ThemeType.Light:
                {
                    yield return new ResourceDictionary
                                 {
                                     Source = new Uri(@"pack://application:,,,/FlatXaml;component/Theme/FlatLightTheme.xaml"),
                                 };
                    yield return new ResourceDictionary
                                 {
                                     Source = new Uri(@"pack://application:,,,/GoldDiff.Shared;component/View/SharedTheme/GoldDiffSharedLightTheme.xaml"),
                                 };
                    break;
                }
                case ThemeType.Dark:
                {
                    yield return new ResourceDictionary
                                 {
                                     Source = new Uri(@"pack://application:,,,/FlatXaml;component/Theme/FlatDarkTheme.xaml"),
                                 };
                    yield return new ResourceDictionary
                                 {
                                     Source = new Uri(@"pack://application:,,,/GoldDiff.Shared;component/View/SharedTheme/GoldDiffSharedDarkTheme.xaml"),
                                 };
                    break;
                }
                default:
                {
                    throw new Exception($"Unknown {nameof(ThemeType)} {Theme}!");
                }
            }
        }
        
        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(StorageLocation)!);
                File.WriteAllText(StorageLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception exception)
            {
                // TODO: implement error handling
            }
        }
    }
}