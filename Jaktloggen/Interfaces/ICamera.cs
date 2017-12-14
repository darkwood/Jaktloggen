using System.IO;
using System.Threading.Tasks;

namespace Jaktloggen.Interfaces
{
    public interface ICamera
    {
        //Task<Stream> GetImageStreamAsync();
        string GetPictureFromDisk(string filename);
        void BringUpCamera();
        void BringUpPhotoGallery();
    }
}
