using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Jaktloggen.Interfaces;

using Plugin.Media;
using Jaktloggen.Services;

namespace Jaktloggen.InputViews
{
    public partial class InputImage : ContentPage
    {
        private ImageSource _source;
        public ImageSource Source
        {
            get
            {
                if (_source != null)
                {
                    return _source;
                }

                if (string.IsNullOrEmpty(Filepath))
                {
                    _source = ImageSource.FromUri(new Uri("http://imaginations.csj.ualberta.ca/wp-content/themes/15zine/library/images/placeholders/placeholder-759x500.png"));
                    //return ImageSource.FromUri(new Uri("http://iliketowastemytime.com/sites/default/files/imagecache/blog_image/Evergreen-Mountain-Lookout-Sunset-by-Michael-Matti.jpg"));
                }
                else
                {
                    //var filepath = DependencyService.Get<ICamera>().GetPictureFromDisk(Filepath);
                    _source = ImageSource.FromFile(Filepath);
                }

                return _source;
            }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
                OnPropertyChanged(nameof(ImageExists));
            }
        }
        private string m_filepath;
        public string Filepath
        {
            get => m_filepath;
            set { 
                m_filepath = value; 
                OnPropertyChanged(nameof(Filepath)); 
                OnPropertyChanged(nameof(Source)); 
                OnPropertyChanged(nameof(ImageExists));
            }
        }

        public bool ImageExists => !string.IsNullOrEmpty(Filepath);

        private Action<InputImage> _callback { get; set; }
        public InputImage(string title, string filepath, Action<InputImage> callback)
        {
            Filepath = filepath;
            Title = title;
            _callback = callback;
            BindingContext = this;


            //MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (bytes) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        //Set the source of the image view with the byte array
            //        Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            //        string f = "img" + DateTime.Now.ToString("yyMMdd-hhmmss");
            //        var filePath = Services.FileService.SaveImage(f, bytes);
            //        Filename = f;
            //    });
            //});

            InitializeComponent();
        }

        public InputImage()
        {
            InitializeComponent();
        }
        async void Camera_Clicked(object sender, System.EventArgs e)
        {
            //DependencyService.Get<ICamera>().BringUpCamera(); 
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Photos"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            _source = null;
            Filepath = file.Path;

            //or:
            //image.Source = ImageSource.FromFile(file.Path);
            //image.Dispose();
        }

        async void Library_Clicked(object sender, System.EventArgs e)
        {
            //DependencyService.Get<ICamera>().BringUpPhotoGallery();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            string destinationPath = file.Path.Replace("/temp/", "/Photos/");
            await FileService.CopyAsync(file.Path, destinationPath);

            _source = null;
            Filepath = destinationPath;
        }

        void Delete_Clicked(object sender, EventArgs e)
        {

        }

        async void Done_Clicked(object sender, EventArgs e)
        {
            await Done();
        }

        public async Task Done()
        {
            _callback(this);
            await Navigation.PopAsync();
        }
    }
}
