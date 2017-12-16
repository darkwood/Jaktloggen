using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jaktloggen.iOS.IO;
using Jaktloggen.Interfaces;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FileUtility))]
namespace Jaktloggen.iOS.IO
{
    public class FileUtility : IFileUtility
    {
        public async Task<string> SaveAsync(string filename, string text)
        {
            string filePath = GetFilePath(filename);
            await WriteDataToDisk(filePath, Encoding.GetEncoding(28591).GetBytes(text));
            return filePath;
        }

        public async Task<string> SaveImageAsync(string filename, byte[] imageData)
        {
            string filePath = GetFilePath(filename);
            await WriteDataToDisk(filePath, imageData);
            return filePath;
        }


        public async Task<string> LoadAsync(string filename)
        {
            string filePath = GetFilePath(filename);
            var result = await ReadDataFromDisk(filePath);
            return Encoding.GetEncoding(28591).GetString(result);
        }


        public bool Exists(string filename)
        {
            string filePath = GetFilePath(filename);
            return File.Exists(filePath);
        }

        public async Task LogErrorAsync(string error)
        {
            string filePath = GetFilePath("error.txt");
            using (FileStream sourceStream = new FileStream(filePath,
                            FileMode.Append, FileAccess.Write, FileShare.None,
                            bufferSize: 4096, useAsync: true))
            {
                byte[] bytes = Encoding.GetEncoding(28591).GetBytes(error);
                await sourceStream.WriteAsync(bytes, 0, bytes.Length);
            };
        }

        public void Delete(string filename)
        {
            if (Exists(filename))
            {
                string filePath = GetFilePath(filename);
                File.Delete(filePath);
            }
        }

        public DateTime GetLastWriteTime(string filename)
        {
            string filePath = GetFilePath(filename);
            return File.GetLastWriteTime(filePath);
        }
        private string GetFilePath(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return filePath;
        }

        public async Task CopyAsync(string sourcePath, string destinationPath)
        {
            using (Stream source = File.Open(sourcePath, FileMode.Open))
            {
                new FileInfo(destinationPath).Directory.Create();
                using (Stream destination = File.Create(destinationPath))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }

        async Task WriteDataToDisk(string filePath, byte[] encodedText)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                            FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,
                            bufferSize: 4096, useAsync: false))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        async Task<byte[]> ReadDataFromDisk(string filePath)
        {
            byte[] result;
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }
            return result;
        }
    }
}