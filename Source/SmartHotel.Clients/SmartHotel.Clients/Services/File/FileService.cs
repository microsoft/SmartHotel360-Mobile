using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SmartHotel.Clients.Core.Services.File
{
    public class FileService : IFileService
    {
        public List<string> GetEmbeddedResourceNames()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FileService)).Assembly;
            var resourceNamesList = new List<string>();

            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                resourceNamesList.Add(resourceName);
            }

            return resourceNamesList;
        }

        public string ReadStringFromAssemblyEmbeddedResource(string resourceName)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FileService)).Assembly;
            var stream = assembly.GetManifestResourceStream(resourceName);

            var resourceContent = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                resourceContent = reader.ReadToEnd();
            }
            
            return resourceContent;
        }

        public string ReadStringFromLocalAppDataFolder(string fileName)
        {
            var fullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            if(!System.IO.File.Exists(fullFileName))
            {
                return null;
            }

            var resourceContent = string.Empty;
            try
            {
                resourceContent = System.IO.File.ReadAllText(fullFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading file from {fullFileName}, reason: {ex.Message}");
            }            

            return resourceContent;
        }

        public bool WriteStringToLocalAppDataFolder(string fileName, string textToWrite)
        {
            var fullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            try
            {
                System.IO.File.WriteAllText(fullFileName, textToWrite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing file to {fullFileName}, reason: {ex.Message}");
                return false;
            }

            return true;
        }

        public bool ExistsInLocalAppDataFolder(string fileName)
        {
            var fullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            return System.IO.File.Exists(fullFileName);
        }

    }
}
