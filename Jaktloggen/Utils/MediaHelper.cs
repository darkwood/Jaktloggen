using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Jaktloggen.Services;
using Plugin.Media;

namespace Jaktloggen
{
    public static class MediaHelper
    {
        public static async Task<string> OpenLibrary(ContentPage page)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await page.DisplayAlert("Bilder støttes ikke", "Tilgang er ikke gitt til bildebiblioteket. Kan endres i innstillinger på telefonen.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large,
                CompressionQuality = 92
            });

            if ((file == null))
            {
                return null;
            }

            string filename = file.Path.Substring(file.Path.LastIndexOf("/", StringComparison.CurrentCulture) + 1);
            FileService.Copy("temp/" + filename, filename);
            return filename;
        }

        public static async Task<string> OpenCamera(ContentPage page)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "",
                CompressionQuality = 92,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large
            });

            if (file == null)
            {
                return null;
            }

            return file.Path;
        }
    }

}