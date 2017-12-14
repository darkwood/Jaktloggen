using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Jaktloggen.Interfaces;

namespace Jaktloggen.InputViews
{
    public partial class InputImage : ContentPage
    {
        private ImageSource _source;
        public ImageSource Source
        {
            get
            {
                if(_source != null){
                    return _source;
                }

                if (string.IsNullOrEmpty(Filename))
                {
                    _source = ImageSource.FromUri(new Uri("http://imaginations.csj.ualberta.ca/wp-content/themes/15zine/library/images/placeholders/placeholder-759x500.png"));
                    //return ImageSource.FromUri(new Uri("http://iliketowastemytime.com/sites/default/files/imagecache/blog_image/Evergreen-Mountain-Lookout-Sunset-by-Michael-Matti.jpg"));
                } else{
                    var filepath = DependencyService.Get<ICamera>().GetPictureFromDisk(Filename);
                    _source = ImageSource.FromFile(filepath);
                }
               
                return _source;
            }
            set{
                _source = value;
                OnPropertyChanged(nameof(Source));
                OnPropertyChanged(nameof(ImageExists));
            }
        }
        private string _filename;
        public string Filename
        {
            get => _filename;
            set { _filename = value; OnPropertyChanged(nameof(Filename)); }
        }

        public bool ImageExists => !string.IsNullOrEmpty(Filename);

        private Action<InputImage> _callback { get; set; }
        public InputImage(string title, string filename, Action<InputImage> callback)
        {
            Filename = filename;
            Title = title;
            _callback = callback;
            BindingContext = this;


            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (bytes) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Set the source of the image view with the byte array
                    Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    string f = "img" + DateTime.Now.ToString("yyMMdd-hhmmss");
                    var filePath = Services.FileService.SaveImage(f, bytes);
                    Filename = f;
                });
            });

            InitializeComponent();
        }

        public InputImage()
        {
            InitializeComponent();
        }
        void Camera_Clicked(object sender, System.EventArgs e)
        {
            DependencyService.Get<ICamera>().BringUpCamera(); 
        }

        void Library_Clicked(object sender, System.EventArgs e)
        {
            DependencyService.Get<ICamera>().BringUpPhotoGallery();
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
