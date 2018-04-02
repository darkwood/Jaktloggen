using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
                return ImageSource.FromFile(App.PLACEHOLDER_PHOTO);
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

        public static string GetImageFilename(string filePath)
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

        public static async Task AnimateButton(VisualElement btn)
        {
            await btn.ScaleTo(0.75, 50, Easing.Linear);
            await btn.ScaleTo(1, 50, Easing.Linear);
        }
    }
}
