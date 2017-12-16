using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen.Interfaces
{
    public interface IFileUtility
    {
        Task<string> SaveAsync(string filename, string text);
        Task<string> SaveImageAsync(string filename, byte[] imageData);
        Task<string> LoadAsync(string filename);
        DateTime GetLastWriteTime(string filename);
        bool Exists(string filename);
        Task LogErrorAsync(string error);
        void Delete(string filename);
        Task CopyAsync(string sourcePath, string destinationPath);
    }
}
