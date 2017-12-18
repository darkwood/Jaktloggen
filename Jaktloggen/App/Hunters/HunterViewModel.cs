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
        public Hunter Hunter { get; set; }
        public Hunter Item { get; set; }
        public Command LoadItemsCommand { get; set; }

        public override string ID
        {
            get { return Item.ID; }
            set { Item.ID = value; OnPropertyChanged(nameof(ID)); }
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
        public ICommand DeleteCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public HunterViewModel(Hunter item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(Name) ? "Ny jeger" : Name;
            CreateCommands(item);
        }

        public async Task OnAppearing()
        {
            if (string.IsNullOrEmpty(Item.ID) && !isLoaded)
            {
                isLoaded = true;
            }
        }
        
        private void CreateCommands(Hunter item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, obj => {
                    ImageFilename = obj.ImageFilename;
                }));
            });
        }
    }
}
