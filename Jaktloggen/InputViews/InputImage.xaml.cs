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

                _source = Utility.GetImageSource(ImageFilename);

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
        public string ImageFilename
        {
            get => m_filepath;
            set
            {
                m_filepath = value;
                OnPropertyChanged(nameof(ImageFilename));
                OnPropertyChanged(nameof(Source));
                OnPropertyChanged(nameof(ImageExists));
            }
        }

        public bool ImageExists => !string.IsNullOrEmpty(ImageFilename);

        private Action<InputImage> _callback { get; set; }
        public InputImage(string title, string filepath, Action<InputImage> callback)
        {
            ImageFilename = filepath;
            Title = title;
            _callback = callback;
            BindingContext = this;

            InitializeComponent();
        }

        public InputImage()
        {
            //Source = ImageSource.FromFile("placeholder_photo.png");
            InitializeComponent();
        }
        async void Camera_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "",
                CompressionQuality = 92,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large
            });

            if (file == null)
                return;
            
            _source = null;
            //string filename = file.Path.Substring(file.Path.LastIndexOf("/", StringComparison.CurrentCulture)+1);
            string filename = file.Path;
            ImageFilename = filename;
        }

        async void Library_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Bilder støttes ikke", "Tilgang er ikke gitt til bildebiblioteket. Kan endres i innstillinger på telefonen.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large,
                CompressionQuality = 92
            });


            if (file == null)
                return;

            string filename = file.Path.Substring(file.Path.LastIndexOf("/", StringComparison.CurrentCulture)+1);
            FileService.Copy("temp/"+filename, filename);

            _source = null;
            ImageFilename = filename;
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
