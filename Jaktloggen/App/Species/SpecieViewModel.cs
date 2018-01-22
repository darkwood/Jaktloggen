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
    public class SpecieViewModel : BaseViewModel
    {
        public Art Item { get; set; }
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
        bool selected;

        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public ICommand NameCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public SpecieViewModel(Art item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(Name) ? "Ny art" : Name;
            CreateCommands(item);
        }
        
        private void CreateCommands(Art item)
        {
            NameCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Navn", Name, obj => {
                    Name = obj.Value;
                }));
            });
        }
    }
}
