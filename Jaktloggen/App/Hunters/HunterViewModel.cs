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
    public class HunterViewModel : BaseViewModel
    {
        public Hunter Item { get; set; }
        public Command LoadItemsCommand { get; set; }

        public override string ID
        {
            get { return Item.ID; }
            set { Item.ID = value; OnPropertyChanged(nameof(ID)); }
        }

        public string Firstname
        {
            get { return Item.Fornavn; }
            set { Item.Fornavn = value; OnPropertyChanged(nameof(Firstname)); }
        }

        public string Lastname
        {
            get { return Item.Etternavn; }
            set { Item.Etternavn = value; OnPropertyChanged(nameof(Lastname)); }
        }

        public string Phone
        {
            get { return Item.Phone; }
            set { Item.Phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        public string Email
        {
            get { return Item.Email; }
            set { Item.Email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string Name => (Item.Fornavn + " " + Item.Etternavn).Trim();

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
        
        public ImageSource Image => Utility.GetImageSource(ImageFilename);
        
        public ICommand ImageCommand { protected set; get; }
        public ICommand FirstnameCommand { protected set; get; }
        public ICommand LastnameCommand { protected set; get; }
        public ICommand PhoneCommand { protected set; get; }
        public ICommand EmailCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public HunterViewModel(Hunter item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(Name) ? "Ny jeger" : Name;
            CreateCommands(item);
        }

        private void CreateCommands(Hunter item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, obj => {
                    ImageFilename = obj.ImageFilename;
                }));
            });
            FirstnameCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Fornavn", Firstname, obj => {
                    Firstname = obj.Value;
                }));
            });
            LastnameCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Etternavn", Lastname, obj => {
                    Lastname = obj.Value;
                }));
            });
            PhoneCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Telefon", Phone, obj => {
                    Phone = obj.Value;
                }));
            });
            EmailCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("E-post", Email, obj => {
                    Email    = obj.Value;
                }));
            });
        }
    }
}
