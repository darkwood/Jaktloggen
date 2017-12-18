using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class LogViewModel : BaseViewModel
    {
        public HuntViewModel Hunt { get; set; }
        public Log Item { get; set; }
        public Command LoadItemsCommand { get; set; }

        public override string ID
        {
            get { return Item.ID; }
            set { Item.ID = value; OnPropertyChanged(nameof(ID)); }
        }

        public DateTime Date
        {
            get { return Item.Date; }
            set { Item.Date = value; OnPropertyChanged(nameof(Date)); }
        }

        public string Latitude
        {
            get { return Item.Latitude; }
            set { Item.Latitude = value; OnPropertyChanged(nameof(Latitude)); }
        }

        public string Longitude
        {
            get { return Item.Longitude; }
            set { Item.Longitude = value; OnPropertyChanged(nameof(Longitude)); }
        }
        public string PositionInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(InfoMessage))
                {
                    return InfoMessage;
                }
                return string.IsNullOrEmpty(Latitude) ? "" : $"{Latitude}, {Longitude}";
            }
        }
        public string ImageFilename
        {
            get => Utility.GetImageFileName(Item.ImagePath);
            set
            {
                Item.ImagePath = value;
                OnPropertyChanged(nameof(ImageFilename));
                OnPropertyChanged(nameof(Image));
            }
        }

        public ImageSource Image
        {
            get
            {
                return Utility.GetImageSource(ImageFilename);
            }
        }

        string notes;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); OnPropertyChanged(nameof(Notes)); }
        }

        string infoMessage;
        public string InfoMessage
        {
            get { return infoMessage; }
            set { SetProperty(ref infoMessage, value); OnPropertyChanged(nameof(PositionInfo)); }
        }

        public ICommand LocationCommand { protected set; get; }
        public ICommand ImageCommand { protected set; get; }
        public ICommand DateCommand { protected set; get; }
        public ICommand NotesCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public LogViewModel(Log item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(item.ID) ? "Ny loggføring" : item.Date.ToString();
            CreateCommands(item);
        }

        public async Task OnAppearing()
        {
            if (string.IsNullOrEmpty(Item.ID) && !isLoaded)
            {
                Date = DateTime.Now;

                await SetPositionAsync();

                isLoaded = true;
            }
        }

        private async Task SetPositionAsync()
        {
            InfoMessage = "Henter din posisjon...";
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5L;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);

                InfoMessage = "";

                Latitude = position.Latitude.ToString();
                Longitude = position.Longitude.ToString();

                OnPropertyChanged(nameof(PositionInfo));
            }
            catch (Exception ex)
            {
                //todo: log this
                throw new Exception(ex.Message, ex);
            }
        }

        private void CreateCommands(Log item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, (InputImage obj) => {
                    ImageFilename = obj.ImageFilename;
                }));
            });

            DateCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Tidspunkt", Date, (InputDate obj) => {
                    Date = obj.Value;
                }));
            });

            NotesCommand = new Command(async () =>
            {
                var inputView = new InputEntry("Notater", Notes, (InputEntry obj) =>
                {
                    Notes = obj.Value;
                });
                inputView.Multiline = true;
                await Navigation.PushAsync(inputView);
            });
        }
    }
}
