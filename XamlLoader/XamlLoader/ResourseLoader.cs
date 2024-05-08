using Noxum.Publishing.ImageConverter.ImageTypeProperties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace XamlLoader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ReadAndWriteXml : Application
    {
        public static ResourceDictionary? Dictionary;
        public static String XamlFileName = @"G:\Work\Personal\XamlLoader\XamlLoader\DictionaryFile.xaml";
        private IEnumerable<ImageTypeInfo> imageTypeInfos;

        public ReadAndWriteXml()
        {           
        }

        /// <summary>
        /// This funtion loads a ResourceDictionary from a file at runtime
        /// </summary>
        public void LoadStyleDictionaryFromFile(string inFileName)
        {
            if (File.Exists(inFileName))
            {
                try
                {
                    using (var fs = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read in ResourceDictionary File
                        Dictionary = (ResourceDictionary)XamlReader.Load(fs);
                        // Clear any previous dictionaries loaded
                        Resources.MergedDictionaries.Clear();
                        // Add in newly loaded Resource Dictionary
                        Resources.MergedDictionaries.Add(Dictionary);
                    }
                }
                catch
                {
                    // Do something here if file not found
                }
            }
        }

        public void LoadImageType(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read in ResourceDictionary File
                        imageTypeInfos = (IEnumerable<ImageTypeInfo>)XamlReader.Load(fs);
                        // Clear any previous dictionaries loaded
                        Resources.MergedDictionaries.Clear();
                        // Add in newly loaded Resource Dictionary
                        Resources.MergedDictionaries.Add(Dictionary);
                    }
                }
                catch
                {
                    // Do something here if file not found
                }
            }
        }

        private void Application_Exit_1(object sender, ExitEventArgs e)
        {
            try
            {
                Dictionary["HW"] += "Hello, back!";
                StreamWriter writer = new StreamWriter(XamlFileName);
                XamlWriter.Save(Dictionary, writer);
            }
            catch
            {
                // Do something here if file not found
            }
        }
    }
}