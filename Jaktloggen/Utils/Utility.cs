using System;
using System.Diagnostics;
using Jaktloggen.Interfaces;
using Xamarin.Forms;

namespace Jaktloggen
{
    public static class Utility
    {
        public static ImageSource GetImageSource(string imageFilename)
        {
            if (String.IsNullOrEmpty(imageFilename))
            {
                return ImageSource.FromFile("placeholder_photo.png");
            }
            else if (imageFilename.StartsWith("/"))
            {
                return ImageSource.FromFile(imageFilename);
            }
            else
            {
                var filepath = DependencyService.Get<IFileUtility>().GetFilePath(imageFilename);
                return ImageSource.FromFile(filepath);
            }
        }

        public static string GetImageFileName(string filePath)
        {
            if (!String.IsNullOrEmpty(filePath) && filePath.Contains("/"))
            {
                filePath = filePath.Substring(filePath.LastIndexOf("/", StringComparison.CurrentCulture) + 1);
            }
            return filePath;
        }
        public static void LogError(Exception ex)
        {
            Debug.WriteLine(ex.Message, ex.InnerException, ex.StackTrace);
        }
    }
}
