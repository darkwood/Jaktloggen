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
    public class DogViewModel : BaseViewModel
    {
        public Dog Item { get; set; }
        public Command LoadItemsCommand { get; set; }

        public override string ID
        {
            get { return Item.ID; }
            set { Item.ID = value; OnPropertyChanged(nameof(ID)); }
        }

        public string Name
        {
            get { return Item.Navn; }
            set { Item.Navn = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Breed
        {
            get { return Item.Rase; }
            set { Item.Rase = value; OnPropertyChanged(nameof(Breed)); }
        }

        public string Number
        {
            get { return Item.Registreringsnummer; }
            set { Item.Registreringsnummer = value; OnPropertyChanged(nameof(Number)); }
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
        
        public ImageSource Image => Utility.GetImageSource(ImageFilename);
        
        public ICommand ImageCommand { protected set; get; }
        public ICommand NameCommand { protected set; get; }
        public ICommand BreedCommand { protected set; get; }
        public ICommand NumberCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public DogViewModel(Dog item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(Name) ? "Ny hund" : Name;
            CreateCommands(item);
        }
        
        private void CreateCommands(Dog item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, obj => {
                    ImageFilename = obj.ImageFilename;
                }));
            });
            NameCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Navn", Name, obj => {
                    Name = obj.Value;
                }));
            });
            BreedCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Rase", Breed, obj => {
                    Breed = obj.Value;
                }));
            });
            NumberCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Reg.nummer", Number, obj => {
                    Number = obj.Value;
                }));
            });
            DeleteCommand = new Command(async () => {
                MessagingCenter.Send(this, "Delete");
                await Navigation.PopAsync();
            });
        }
    }
}
