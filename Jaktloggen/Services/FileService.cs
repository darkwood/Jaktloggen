using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jaktloggen.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Jaktloggen.Services
{
    public static class FileService
    {
        public static async Task SaveToLocalStorage<T>(this T objToSerialize, string filename)
        {
            if (filename.ToLower().EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                var jsonString = JsonConvert.SerializeObject(objToSerialize);
                await DependencyService.Get<IFileUtility>().SaveAsync(filename, jsonString);
                return;
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objToSerialize.GetType());

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, objToSerialize);
                    await DependencyService.Get<IFileUtility>().SaveAsync(filename, textWriter.ToString());
                }
            }
        }

        public static async Task<T> LoadFromLocalStorage<T>(string filename, bool loadFromserver = false)
        {
            var localObj = (T)Activator.CreateInstance(typeof(T));
            // 1 read json
            if (filename.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase) && Exists(filename))
            {
                var jsonString = await DependencyService.Get<IFileUtility>().LoadAsync(filename);

                try
                {
                    localObj = JsonConvert.DeserializeObject<T>(jsonString);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                    //Utils.LogError(ex);
                }
            }
            else
            {
                //try to read from legacy xml
                var xmlFilename = filename.Replace(".json", ".xml");
                var localFileExists = Exists(xmlFilename);

                if (localFileExists)
                {
                    var xmlString = await DependencyService.Get<IFileUtility>().LoadAsync(xmlFilename);
                    try
                    {
                        using (var reader = new StringReader(xmlString))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            localObj = (T)serializer.Deserialize(reader);
                            await localObj.SaveToLocalStorage(filename); //Save to json format
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                        //Utils.LogError(ex);
                    }
                }
            }
            return localObj;
        }

        public static bool Exists(string filename)
        {
            return DependencyService.Get<IFileUtility>().Exists(filename);
        }

        public static DateTime GetLastWriteTime(string filename)
        {
            return DependencyService.Get<IFileUtility>().GetLastWriteTime(filename);
        }

        public static async Task CopyToAppFolderAsync(string file)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Xml." + file);

            using (var reader = new StreamReader(stream))
            {
                await DependencyService.Get<IFileUtility>().SaveAsync(file, reader.ReadToEnd());
            }
        }

        public static async Task<string> SaveImage(string filename, byte[] imageData)
        {
            return await DependencyService.Get<IFileUtility>().SaveImageAsync(filename, imageData);
        }

        public static async Task CopyAsync(string sourcePath, string destinationPath) 
            => await DependencyService.Get<IFileUtility>().CopyAsync(sourcePath, destinationPath);
    }
}
