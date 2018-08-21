using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Jaktloggen.Interfaces;

using Plugin.Media;
using Jaktloggen.Services;
using ImageCircle.Forms.Plugin.Abstractions;

namespace Jaktloggen.InputViews
{
    public partial class InputImage : ContentPage
    {
        private ImageSource _source;
        public ImageSource Source
        {
            get => _source ?? Utility.GetImageSource(ImageFilename);
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        private string m_filepath;
        public string ImageFilename
        {
            get => m_filepath;
            set
            {
                _source = null;
                m_filepath = value;
                IsDirty = true;
                OnPropertyChanged(nameof(ImageFilename));
                OnPropertyChanged(nameof(Source));
            }
        }

        bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public bool IsDirty
        {
            get;
            set;
        }

        private Action<InputImage> _callback { get; set; }

        public InputImage(string title, string filepath, Action<InputImage> callback)
        {
            m_filepath = filepath;
            Title = title;
            _callback = callback;
            BindingContext = this;

            InitializeComponent();
        }

        public InputImage()
        {
            InitializeComponent();
        }

        public async Task Initialize(string arg)
        {
            if (arg == "takephoto") { await OpenCamera(this); }
            if (arg == "openlibrary") { await OpenLibrary(this); }
        }

		async void Camera_Clicked(object sender, System.EventArgs e)
        {
            await OpenCamera(sender);
        }

        private async Task OpenCamera(object sender)
        {
            IsLoading = true;
            if (sender != null)
            {
                await Utility.AnimateButton((VisualElement)sender);
            }

            var filename = await MediaHelper.OpenCamera(this);
            if (filename != null)
            {
                ImageFilename = filename;
            }
            IsLoading = false;
        }


        async void Library_Clicked(object sender, System.EventArgs e)
        {
            await OpenLibrary(sender);
        }

        private async Task OpenLibrary(object sender)
        {
            IsLoading = true;
            await Utility.AnimateButton(sender as VisualElement);

            var filename = await MediaHelper.OpenLibrary(this);
            if (filename != null)
            {
                ImageFilename = filename;
            }

            IsLoading = false;
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            await Utility.AnimateButton((VisualElement)sender);

            ImageFilename = null;
        }

        async void Done_Clicked(object sender, EventArgs e)
        {
            await Done();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            if(!IsDirty || await DisplayAlert("Bekreft avbryt", "Alle endringer vil gå tapt", "OK", "Avbryt"))
            {
                await Navigation.PopAsync();
            }
        }

        public async Task Done()
        {
            _callback(this);
            await Navigation.PopAsync();
        }

    }
}
