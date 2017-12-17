using System;
using Jaktloggen.Interfaces;
using Xamarin.Forms;

namespace Jaktloggen
{
    public static class Utility
    {
        public static ImageSource GetImageSource(string imageFilename)
        {
            if (string.IsNullOrEmpty(imageFilename))
            {
                return ImageSource.FromUri(new Uri("http://imaginations.csj.ualberta.ca/wp-content/themes/15zine/library/images/placeholders/placeholder-759x500.png"));
            }
            var filepath = DependencyService.Get<IFileUtility>().GetFilePath(imageFilename);
            return ImageSource.FromFile(filepath);
        }

    }
}
